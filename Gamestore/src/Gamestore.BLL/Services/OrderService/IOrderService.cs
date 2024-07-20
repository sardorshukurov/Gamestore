using Gamestore.BLL.DTOs.Order;

namespace Gamestore.BLL.Services.OrderService;

public interface IOrderService
{
    Task AddGameInTheCartAsync(Guid customerId, string gameKey);

    Task RemoveGameFromTheCartAsync(Guid customerId, string gameKey);

    Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrdersAsync();

    Task<OrderDto?> GetByIdAsync(Guid orderId);

    Task<IEnumerable<OrderDetailsDto>> GetOrderDetailsAsync(Guid orderId);

    Task<IEnumerable<OrderDetailsDto>> GetCartAsync(Guid customerId);

    Task CancelOrderAsync(Guid orderId);

    Task PayOrderAsync(Guid orderId);

    Task<byte[]> GenerateInvoicePdfAsync(Guid customerId);

    Task<double> GetCartSumAsync(Guid customerId);

    Task<Guid> GetCartIdAsync(Guid customerId);
}