using Gamestore.BLL.DTOs.Order;

namespace Gamestore.BLL.Services.OrderService;

public interface IOrderService
{
    Task AddGameInTheCartAsync(Guid customerId, string gameKey);

    Task RemoveGameFromTheCartAsync(Guid customerId, string gameKey);

    Task<IEnumerable<OrderResponse>> GetPaidAndCancelledOrdersAsync();

    Task<OrderResponse?> GetByIdAsync(Guid orderId);

    Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsAsync(Guid orderId);

    Task<IEnumerable<OrderDetailsResponse>> GetCartAsync(Guid customerId);

    Task CancelOrderAsync(Guid orderId);

    Task PayOrderAsync(Guid orderId);

    Task<byte[]> GenerateInvoicePdfAsync(Guid customerId);

    Task<double> GetCartSumAsync(Guid customerId);

    Task<Guid> GetCartIdAsync(Guid customerId);
}