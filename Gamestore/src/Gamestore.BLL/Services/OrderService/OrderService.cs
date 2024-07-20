using Gamestore.BLL.DTOs.Order;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Services.OrderService;

public class OrderService(IRepository<Order> orderRepository, IRepository<OrderGame> orderGameRepository, IRepository<Game> gameRepository) : IOrderService
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IRepository<OrderGame> _orderGameRepository = orderGameRepository;
    private readonly IRepository<Game> _gameRepository = gameRepository;

    public async Task AddGameInTheCart(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        EnsureGameIsInStock(game);

        var order = await GetOrCreateOpenOrder(customerId);

        await AddGameToOrder(game, order);
        game.UnitInStock--;

        await _orderRepository.SaveChangesAsync();
    }

    public async Task RemoveGameFromTheCart(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        var order = await GetOrderOrElseThrow(customerId);
        var orderGame = await GetOrderGameOrElseThrow(game.Id, order.Id);

        await _orderGameRepository.DeleteOneAsync(og =>
            og.OrderId == orderGame.OrderId
            && og.ProductId == orderGame.ProductId);

        game.UnitInStock += orderGame.Quantity;

        await _orderRepository.SaveChangesAsync();

        await DeleteOrderIfNoGamesLeft(order.Id);
    }

    public async Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrders(Guid customerId)
    {
        var orders =
            (await _orderRepository.GetAllByFilterAsync(o =>
                (o.Status == OrderStatus.Paid || o.Status == OrderStatus.Cancelled)
                && o.CustomerId == customerId))
            .Select(o => o.AsDto());

        return orders;
    }

    public async Task<OrderDto?> GetById(Guid orderId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.Id == orderId);

        return order?.AsDto();
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetOrderDetails(Guid orderId)
    {
        var orderGames =
            (await _orderGameRepository.GetAllByFilterAsync(og =>
                og.OrderId == orderId))
            .Select(og => og.AsDto());

        return orderGames;
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetCart(Guid customerId)
    {
        var order = await _orderRepository.GetOneAsync(o =>
            o.CustomerId == customerId && o.Status == OrderStatus.Open);

        var orderGames = (await _orderGameRepository.GetAllByFilterAsync(
            og => og.OrderId == order.Id))
            .Select(og => og.AsDto());

        return orderGames;
    }

    private async Task<Game> GetGameOrElseThrow(string gameKey)
    {
        return await _gameRepository.GetOneAsync(g => g.Key == gameKey) ?? throw new GameNotFoundException(gameKey);
    }

    private static void EnsureGameIsInStock(Game game)
    {
        if (game.UnitInStock < 1)
        {
            throw new NotEnoughGamesInStockException($"There are not enough {game.Name} games in stock");
        }
    }

    private async Task<Order> GetOrCreateOpenOrder(Guid customerId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.Status == OrderStatus.Open
                                                            && o.CustomerId == customerId);
        if (order is null)
        {
            order = new Order()
            {
                CustomerId = customerId,
                Date = DateTime.Now,
                Status = OrderStatus.Open,
            };
            await _orderRepository.CreateAsync(order);
        }

        return order;
    }

    private async Task AddGameToOrder(Game game, Order order)
    {
        var orderGame = await _orderGameRepository.GetOneAsync(og => og.OrderId == order.Id && og.ProductId == game.Id);
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
            await _orderGameRepository.CreateAsync(orderGame);
        }
        else
        {
            orderGame.Quantity++;
        }
    }

    private async Task<Order> GetOrderOrElseThrow(Guid customerId)
    {
        return await _orderRepository.GetOneAsync(o => o.Status == OrderStatus.Open && o.CustomerId == customerId)
               ?? throw new OrderNotFoundException(customerId);
    }

    private async Task<OrderGame> GetOrderGameOrElseThrow(Guid gameId, Guid orderId)
    {
        return await _orderGameRepository.GetOneAsync(og => og.OrderId == orderId
                                                            && og.ProductId == gameId)
               ?? throw new OrderNotFoundException(gameId);
    }

    private async Task DeleteOrderIfNoGamesLeft(Guid orderId)
    {
        if (await _orderGameRepository.CountByFilterAsync(og => og.OrderId == orderId) == 0)
        {
            await _orderRepository.DeleteByIdAsync(orderId);
            await _orderRepository.SaveChangesAsync();
        }
    }
}