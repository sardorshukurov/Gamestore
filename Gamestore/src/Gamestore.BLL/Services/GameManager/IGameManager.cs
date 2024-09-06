using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Order;

namespace Gamestore.BLL.Services.GameManager;

public interface IGameManager
{
    Task<IEnumerable<OrderResponse>> GetOrdersHistoryAsync(OrderHistoryOptions orderHistoryOptions);

    Task<IEnumerable<GameResponse>> GetAllGamesAsync();

    Task AddGameInTheCartAsync(Guid customerId, string gameKey);

    Task RemoveGameFromTheCartAsync(Guid customerId, string gameKey);
}