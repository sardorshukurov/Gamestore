using Gamestore.BLL.DTOs.Order;

namespace Gamestore.BLL.Services.OrderService;

public interface IOrderService
{
    Task AddGameInTheCart(Guid customerId, string gameKey);

    Task RemoveGameFromTheCart(Guid customerId, string gameKey);

    Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrders(Guid customerId);

    Task<OrderDto?> GetById(Guid orderId);

    Task<IEnumerable<OrderDetailsDto>> GetOrderDetails(Guid orderId);

    Task<IEnumerable<OrderDetailsDto>> GetCart(Guid customerId);
}