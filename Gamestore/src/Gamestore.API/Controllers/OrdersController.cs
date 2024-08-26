using Gamestore.BLL.DTOs.Order;
using Gamestore.BLL.DTOs.Order.Payment;
using Gamestore.BLL.Payments;
using Gamestore.BLL.Services.GameManager;
using Gamestore.BLL.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(
    IOrderService orderService,
    IHttpClientFactory httpClientFactory,
    ILogger<OrdersController> logger,
    IGameManager gameManager) : ControllerBase
{
    private const string BankPaymentMethod = "Bank";
    private const string IBoxPaymentMethod = "IBox terminal";
    private const string VisaPaymentMethod = "Visa";

    // httpclient to make calls to payment microservice
    private readonly HttpClient _paymentClient = httpClientFactory.CreateClient("PaymentAPI");

    private readonly Guid _customerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    [HttpDelete("cart/{key}")]
    public async Task<IActionResult> RemoveGameFromCart(string key)
    {
        await gameManager.RemoveGameFromTheCartAsync(_customerId, key);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetPaidAndCancelledOrders()
    {
        var orders = await orderService.GetPaidAndCancelledOrdersAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse?>> GetById(Guid id)
    {
        var order = await orderService.GetByIdAsync(id);

        return order is null ? NotFound($"Order with id {id} not found") : order;
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<IEnumerable<OrderDetailsResponse>>> GetOrderDetails(Guid id)
    {
        var orderDetails = await orderService.GetOrderDetailsAsync(id);
        return Ok(orderDetails);
    }

    [HttpGet("cart")]
    public async Task<ActionResult<IEnumerable<OrderDetailsResponse>>> GetCart()
    {
        var orderDetails = await orderService.GetCartAsync(_customerId);

        return Ok(orderDetails);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersHistory([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var orders = await gameManager.GetOrdersHistoryAsync(
            start ?? DateTime.MinValue,
            end ?? DateTime.MaxValue);

        return Ok(orders);
    }

    [HttpGet("payment-methods")]
    public async Task<ActionResult<IEnumerable<PaymentMethodResponse>>> GetPaymentMethods()
    {
        return Ok(await orderService.GetPaymentMethodsAsync());
    }

    [HttpPost("payment")]
    public async Task<IActionResult> MakePayment(PaymentRequest request)
    {
        return request.Method switch
        {
            BankPaymentMethod => await ProcessBankPayment(),
            IBoxPaymentMethod => await ProcessIBoxPayment(),
            VisaPaymentMethod => await ProcessVisaPayment(request),
            _ => BadRequest(),
        };
    }

    [HttpPost("{key}/buyGame")]
    public async Task<IActionResult> BuyGame(string key)
    {
        await gameManager.AddGameInTheCartAsync(_customerId, key);
        return Ok();
    }

    private async Task<IActionResult> ProcessVisaPayment(PaymentRequest request)
    {
        var paymentProcessor =
            new VisaPaymentProcessor(orderService, _paymentClient, _customerId, logger, request.Model);

        var result = await paymentProcessor.ProcessPayment();

        return result.Success ? Ok(result.Message) : StatusCode(500, result.Message);
    }

    private async Task<IActionResult> ProcessIBoxPayment()
    {
        var paymentProcessor =
            new IBoxPaymentProcessor(orderService, _paymentClient, _customerId, logger);

        var result = await paymentProcessor.ProcessPayment();

        return result.Success ? Ok(result.Message) : StatusCode(500, result.Message);
    }

    private async Task<FileContentResult> ProcessBankPayment()
    {
        return File(
            await orderService.GenerateInvoicePdfAsync(_customerId),
            "application/pdf",
            "invoice.pdf");
    }
}