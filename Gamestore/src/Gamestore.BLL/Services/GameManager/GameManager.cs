using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Order;
using Gamestore.Common.Exceptions.BadRequest;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;
using Gamestore.Domain.Entities.Northwind;
using Order = Gamestore.Domain.Entities.Order;
using OrderNorthwind = Gamestore.Domain.Entities.Northwind.Order;

namespace Gamestore.BLL.Services.GameManager;

public class GameManager(
    IRepository<Order> orderRepository,
    IMongoRepository<OrderNorthwind> orderNorthwindRepository,
    IRepository<Game> gameRepository,
    IMongoRepository<Product> productNorthwindRepository,
    IRepository<OrderGame> orderGameRepository)
    : IGameManager
{
    public async Task<IEnumerable<OrderResponse>> GetOrdersHistoryAsync(OrderHistoryOptions orderHistoryOptions)
    {
        // Defining tasks without awaiting them immediately allows them to run concurrently
        var orderTask = orderRepository.GetAllByFilterAsync(o => o.Date >= orderHistoryOptions.Start && o.Date <= orderHistoryOptions.End);
        var northwindOrderTask = orderNorthwindRepository.GetAllByFilterAsync(o => o.OrderDate >= orderHistoryOptions.Start && o.OrderDate <= orderHistoryOptions.End);

        // Await all tasks to complete
        await Task.WhenAll(orderTask, northwindOrderTask);

        // Process results after all tasks have completed
        var orders = orderTask.Result.Select(o => o.ToResponse());
        var northwindOrders = northwindOrderTask.Result.Select(o => o.ToResponse());

        var result = orders.Concat(northwindOrders);

        return result;
    }

    public async Task<IEnumerable<GameResponse>> GetAllGamesAsync()
    {
        var northwindGamesTask = productNorthwindRepository.GetAllAsync();

        var gameStoreGamesTask = gameRepository.GetAllAsync();

        await Task.WhenAll(northwindGamesTask, gameStoreGamesTask);

        var northwindGames = northwindGamesTask.Result.Select(ng => ng.ToGamestoreEntity()).ToList();
        var games = gameStoreGamesTask.Result;

        var gameMap = new Dictionary<string, Game>();

        foreach (var game in games)
        {
            gameMap.TryAdd(game.Key, game);
        }

        foreach (var ng in northwindGames)
        {
            gameMap.TryAdd(ng.Key, ng);
        }

        return gameMap.Values.Select(g => g.ToResponse());
    }

    public async Task AddGameInTheCartAsync(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        EnsureGameIsInStock(game);

        var order = await GetOrCreateOpenOrder(customerId);

        await AddGameToOrder(game, order);
        game.UnitInStock--;

        if (game.OriginalId is null)
        {
            var product = await productNorthwindRepository.GetByFilterAsync(g => g.ProductId == game.OriginalId);
            product.UnitsInStock--;
            await productNorthwindRepository.UpdateAsync(product);
        }

        await orderRepository.SaveChangesAsync();
    }

    public async Task RemoveGameFromTheCartAsync(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        var order = await GetOpenOrderByCustomerIdOrElseThrow(customerId);
        var orderGame = await GetOrderGameOrElseThrow(game.Id, order.Id);

        await orderGameRepository.DeleteOneAsync(og =>
            og.OrderId == orderGame.OrderId
            && og.ProductId == orderGame.ProductId);

        game.UnitInStock += orderGame.Quantity;

        if (game.OriginalId is not null)
        {
            var product = await productNorthwindRepository.GetByFilterAsync(g => g.ProductId == game.OriginalId);
            product.UnitsInStock += orderGame.Quantity;
            await productNorthwindRepository.UpdateAsync(product);
        }

        await orderRepository.SaveChangesAsync();

        await DeleteOrderIfNoGamesLeft(order.Id);
    }

    /// <summary>
    /// Gets the game by game key or throws an exception.
    /// </summary>
    /// <param name="gameKey">The game key.</param>
    /// <returns>Game.</returns>
    /// <exception cref="GameNotFoundException">Exception is thrown when the game is not found.</exception>
    private async Task<Game> GetGameOrElseThrow(string gameKey)
    {
        var game = await gameRepository.GetOneAsync(g => g.Key == gameKey);

        if (game is not null)
        {
            return game;
        }

        var product = await productNorthwindRepository.GetByFilterAsync(g => g.GameKey == gameKey)
                      ?? throw new GameNotFoundException(gameKey);

        await gameRepository.CreateAsync(product.ToGamestoreEntity());
        await gameRepository.SaveChangesAsync();

        return await gameRepository.GetOneAsync(g => g.Key == gameKey)!;
    }

    /// <summary>
    /// Checks whether the game has enough units in stock.
    /// </summary>
    /// <param name="game">The game.</param>
    /// <exception cref="NotEnoughGamesInStockException">Exception is thrown when there is not enough units (less than 1).</exception>
    private static void EnsureGameIsInStock(Game game)
    {
        if (game.UnitInStock < 1)
        {
            throw new NotEnoughGamesInStockException($"There are not enough {game.Name} games in stock");
        }
    }

    /// <summary>
    /// Returns order if there is open order for chosen customer otherwise creates a new open order.
    /// </summary>
    /// <param name="customerId">Id of the customer.</param>
    /// <returns>Open order.</returns>
    private async Task<Order> GetOrCreateOpenOrder(Guid customerId)
    {
        var order = await orderRepository.GetOneAsync(o => o.Status == OrderStatus.Open
                                                            && o.CustomerId == customerId);
        if (order is null)
        {
            order = new Order()
            {
                CustomerId = customerId,
                Date = DateTime.Now,
                Status = OrderStatus.Open,
            };
            await orderRepository.CreateAsync(order);
        }

        return order;
    }

    /// <summary>
    /// Adds game to the cart (order).
    /// </summary>
    /// <param name="game">Game, which needs to be added.</param>
    /// <param name="order">Cart (order) to where the game is being added.</param>
    private async Task AddGameToOrder(Game game, Order order)
    {
        var orderGame = await orderGameRepository.GetOneAsync(og => og.OrderId == order.Id
                                                                    && og.ProductId == game.Id);

        if (orderGame is null)
        {
            orderGame = new OrderGame
            {
                OrderId = order.Id,
                ProductId = game.Id,
                Price = game.Price,
                Quantity = 1,
                Discount = game.Discount,
            };
            await orderGameRepository.CreateAsync(orderGame);
        }
        else
        {
            // if the game is already in the cart, the quantity is incremented
            orderGame.Quantity++;
        }
    }

    /// <summary>
    /// Returns open order for the customer or throws exception if order doest not exist.
    /// </summary>
    /// <param name="customerId">Id of the customer.</param>
    /// <returns>Open order.</returns>
    /// <exception cref="OrderNotFoundException">Exception is thrown when open order for the customer is not found.</exception>
    private async Task<Order> GetOpenOrderByCustomerIdOrElseThrow(Guid customerId)
    {
        return await orderRepository.GetOneAsync(o => o.Status == OrderStatus.Open && o.CustomerId == customerId)
               ?? throw new OrderNotFoundException($"Open order for customer with id: {customerId} not found");
    }

    /// <summary>
    /// Returns order game by gameId and orderId or throws exception if not found.
    /// </summary>
    /// <param name="gameId">Id of the game.</param>
    /// <param name="orderId">Id of the order.</param>
    /// <returns>Order game.</returns>
    /// <exception cref="OrderNotFoundException">Exception is thrown when the order game is not found.</exception>
    private async Task<OrderGame> GetOrderGameOrElseThrow(Guid gameId, Guid orderId)
    {
        return await orderGameRepository.GetOneAsync(og => og.OrderId == orderId
                                                            && og.ProductId == gameId)
               ?? throw new OrderNotFoundException($"Game with id {gameId} in order with id {orderId} not found");
    }

    /// <summary>
    /// Deletes order if there are no games left in it.
    /// </summary>
    /// <param name="orderId">Order Id.</param>
    private async Task DeleteOrderIfNoGamesLeft(Guid orderId)
    {
        if (await orderGameRepository.CountByFilterAsync(og => og.OrderId == orderId) == 0)
        {
            await orderRepository.DeleteByIdAsync(orderId);
            await orderRepository.SaveChangesAsync();
        }
    }
}