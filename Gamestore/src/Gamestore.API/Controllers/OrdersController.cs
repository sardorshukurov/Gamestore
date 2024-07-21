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
    IHttpClientFactory httpClientFactory) : ControllerBase
{
    private const int MaxRetryAttempts = 3;

    // httpclient to make calls to payment microservice
    private readonly HttpClient _paymentClient = httpClientFactory.CreateClient("PaymentAPI");

    private readonly Guid _customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetPaidAndCancelledOrders()
    {
        try
        {
            var orders = (await orderService.GetPaidAndCancelledOrdersAsync())
                .Select(o => o.AsResponse());

            return Ok(orders);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse?>> GetById(Guid id)
    {
        try
        {
            var order = await orderService.GetByIdAsync(id);

            return order is null ? NotFound($"Order with id {id} not found") : order.AsResponse();
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<IEnumerable<OrderDetailsResponse>>> GetOrderDetails(Guid id)
    {
        try
        {
            var orderDetails = (await orderService.GetOrderDetailsAsync(id))
                .Select(od => od.AsResponse());

            return Ok(orderDetails);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
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

        while (attempt < MaxRetryAttempts)
        {
            // make the request
            var result = await _paymentClient.PostAsync(_paymentClient.BaseAddress + "/visa", stringContent);

            // if successful make the order paid and return ok response
            if (result.IsSuccessStatusCode)
            {
                var response = new PaymentResponse(_customerId, orderId, DateTime.Now, cartSum);
                await orderService.PayOrderAsync(orderId);
                return Ok(response);
            }

            attempt++;
        }

        // else cancel the order and return bad request with error from microservice api
        await orderService.CancelOrderAsync(orderId);
        return BadRequest($"Payment failed after: {attempt} attempts");
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

        while (attempt < MaxRetryAttempts)
        {
            // make the request
            var result = await _paymentClient.PostAsync(_paymentClient.BaseAddress + "/ibox", stringContent);

            if (result.IsSuccessStatusCode)
            {
                var response = new PaymentResponse(_customerId, orderId, DateTime.Now, cartSum);
                await orderService.PayOrderAsync(orderId);
                return Ok(response);
            }

            attempt++;
        }

        await orderService.CancelOrderAsync(orderId);
        return BadRequest($"Payment failed after: {attempt} attempts");
    }

    private async Task<FileContentResult> ProcessBankPayment()
    {
        return File(
            await orderService.GenerateInvoicePdfAsync(_customerId),
            "application/pdf",
            "invoice.pdf");
    }
}