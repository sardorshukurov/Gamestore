using System.Net;
using System.Text;
using Gamestore.API.DTOs.Order;
using Gamestore.API.DTOs.Order.Payment;
using Gamestore.BLL.Services.OrderService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(
    IOrderService orderService,
    IHttpClientFactory httpClientFactory,
    ILogger<OrdersController> logger) : ControllerBase
{
    private const int MaxRetryAttempts = 3;

    // httpclient to make calls to payment microservice
    private readonly HttpClient _paymentClient = httpClientFactory.CreateClient("PaymentAPI");

    private readonly Guid _customerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    [HttpDelete("cart/{key}")]
    public async Task<IActionResult> RemoveGameFromCart(string key)
    {
        try
        {
            await orderService.RemoveGameFromTheCartAsync(_customerId, key);
            return Ok();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetPaidAndCancelledOrders()
    {
        var orders = (await orderService.GetPaidAndCancelledOrdersAsync())
            .Select(o => o.AsResponse());

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse?>> GetById(Guid id)
    {
        var order = await orderService.GetByIdAsync(id);

        return order is null ? NotFound($"Order with id {id} not found") : order.AsResponse();
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<IEnumerable<OrderDetailsResponse>>> GetOrderDetails(Guid id)
    {
        var orderDetails = (await orderService.GetOrderDetailsAsync(id))
            .Select(od => od.AsResponse());

        return Ok(orderDetails);
    }

    [HttpGet("cart")]
    public async Task<ActionResult<IEnumerable<OrderDetailsResponse>>> GetCart()
    {
        try
        {
            var orderDetails = (await orderService.GetCartAsync(_customerId))
                .Select(od => od.AsResponse());

            return Ok(orderDetails);
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpGet("payment-methods")]
    public ActionResult<IEnumerable<PaymentMethodResponse>> GetPaymentMethods()
    {
        return Ok(PaymentMethodsHelper.PaymentMethods);
    }

    [HttpPost("payment")]
    public async Task<IActionResult> MakePayment(PaymentRequest request)
    {
        try
        {
            return request.Method switch
            {
                "Bank" => await ProcessBankPayment(),
                "IBox terminal" => await ProcessIBoxPayment(),
                "Visa" => await ProcessVisaPayment(request),
                _ => BadRequest(),
            };
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    private async Task<IActionResult> ProcessVisaPayment(PaymentRequest request)
    {
        var orderId = await orderService.GetCartIdAsync(_customerId);
        var cartSum = await orderService.GetCartSumAsync(_customerId);

        // create request for the payment api
        var requestToPaymentApi = new VisaRequest(
            cartSum,
            request.Model.Holder,
            request.Model.CardNumber,
            request.Model.MonthExpire,
            request.Model.Cvv2,
            request.Model.YearExpire);

        // serialize the request
        var serializedRequest = JsonConvert.SerializeObject(requestToPaymentApi);
        StringContent stringContent = new(serializedRequest, Encoding.UTF8, "application/json");

        var attempt = 0;

        var result = new HttpResponseMessage();

        while (attempt < MaxRetryAttempts)
        {
            // make the request
            result = await _paymentClient.PostAsync(_paymentClient.BaseAddress + "/visa", stringContent);

            // if successful make the order paid and return ok response
            if (result.IsSuccessStatusCode)
            {
                var response = new PaymentResponse(_customerId, orderId, DateTime.Now, cartSum);
                await orderService.PayOrderAsync(orderId);
                return Ok(response);
            }

            attempt++;

            logger.LogWarning($"Attempt for payment number {attempt}. Result: {await result.Content.ReadAsStringAsync()}");

            // wait before retrying
            await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
        }

        await orderService.CancelOrderAsync(orderId);

        var message = new StringBuilder();
        message.Append($"Payment failed after: {attempt} attempts. ");

        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            message.Append(await result.Content.ReadAsStringAsync());
        }

        return BadRequest(message.ToString());
    }

    private async Task<IActionResult> ProcessIBoxPayment()
    {
        var orderId = await orderService.GetCartIdAsync(_customerId);
        var cartSum = await orderService.GetCartSumAsync(_customerId);

        var requestToPaymentApi = new IBoxRequest(cartSum, _customerId, orderId);

        // serialize the request
        var serializedRequest = JsonConvert.SerializeObject(requestToPaymentApi);
        StringContent stringContent = new(serializedRequest, Encoding.UTF8, "application/json");

        var attempt = 0;
        var result = new HttpResponseMessage();

        while (attempt < MaxRetryAttempts)
        {
            // make the request
            result = await _paymentClient.PostAsync(_paymentClient.BaseAddress + "/ibox", stringContent);

            if (result.IsSuccessStatusCode)
            {
                var response = new PaymentResponse(_customerId, orderId, DateTime.Now, cartSum);
                await orderService.PayOrderAsync(orderId);
                return Ok(response);
            }

            attempt++;

            logger.LogWarning($"Attempt for payment number {attempt}. Result: {await result.Content.ReadAsStringAsync()}");

            // wait before retrying
            await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
        }

        await orderService.CancelOrderAsync(orderId);

        var message = new StringBuilder();
        message.Append($"Payment failed after: {attempt} attempts. ");

        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            message.Append(await result.Content.ReadAsStringAsync());
        }

        return BadRequest(message.ToString());
    }

    private async Task<FileContentResult> ProcessBankPayment()
    {
        return File(
            await orderService.GenerateInvoicePdfAsync(_customerId),
            "application/pdf",
            "invoice.pdf");
    }
}