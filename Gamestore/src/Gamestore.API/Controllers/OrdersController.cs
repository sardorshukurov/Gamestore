using Gamestore.API.DTOs.Order;
using Gamestore.BLL.Services.OrderService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    // private const string PAYMENTAPI = "http://localhost:5000/api/payments";
    private readonly Guid _customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private readonly IOrderService _orderService = orderService;

    [HttpDelete("cart/{key}")]
    public async Task<IActionResult> RemoveGameFromCart(string key)
    {
        try
        {
            await _orderService.RemoveGameFromTheCartAsync(_customerId, key);
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
            var orders = (await _orderService.GetPaidAndCancelledOrdersAsync())
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
            var order = await _orderService.GetByIdAsync(id);

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
            var orderDetails = (await _orderService.GetOrderDetailsAsync(id))
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
            var orderDetails = (await _orderService.GetCartAsync(_customerId))
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

    // [HttpPost("payment")]
    // public async Task<IActionResult> MakePayment([FromBody] string method)
    // {
    //     if (method == "Bank")
    //     {
    //
    //     }
    // }
}