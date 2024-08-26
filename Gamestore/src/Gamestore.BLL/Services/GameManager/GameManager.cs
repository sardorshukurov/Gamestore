using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Order;
using Gamestore.DAL.Repository;
using Order = Gamestore.Domain.Entities.Order;
using OrderNorthwind = Gamestore.Domain.Entities.Northwind.Order;

namespace Gamestore.BLL.Services.GameManager;

public class GameManager(
    IRepository<Order> orderRepository,
    IMongoRepository<OrderNorthwind> orderNorthwindRepository)
    : IGameManager
{
    public async Task<IEnumerable<OrderResponse>> GetOrdersHistoryAsync(DateTime start, DateTime end)
    {
        var orders = (await orderRepository.GetAllByFilterAsync(
                o => o.Date >= start && o.Date <= end))
            .Select(o => o.ToResponse());

        var northwindOrders = (await orderNorthwindRepository.GetAllByFilterAsync(
            o => o.OrderDate >= start && o.OrderDate <= end))
            .Select(o => o.ToResponse());

        var result = new List<OrderResponse>();
        result.AddRange(orders);
        result.AddRange(northwindOrders);

        return result;
    }

    public Task<IEnumerable<GameResponse>> GetAllGamesAsync()
    {
        throw new NotImplementedException();
    }

    public Task DuplicateProductToGameStore(int productId)
    {
        throw new NotImplementedException();
    }
}