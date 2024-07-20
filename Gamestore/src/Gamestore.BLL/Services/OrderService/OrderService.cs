using Gamestore.BLL.DTOs.Order;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;
using Microsoft.Extensions.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Gamestore.BLL.Services.OrderService;

public class OrderService(
    IRepository<Order> orderRepository,
    IRepository<OrderGame> orderGameRepository,
    IRepository<Game> gameRepository,
    IConfiguration configuration) : IOrderService
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IRepository<OrderGame> _orderGameRepository = orderGameRepository;
    private readonly IRepository<Game> _gameRepository = gameRepository;

    public async Task AddGameInTheCartAsync(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        EnsureGameIsInStock(game);

        var order = await GetOrCreateOpenOrder(customerId);

        await AddGameToOrder(game, order);
        game.UnitInStock--;

        await _orderRepository.SaveChangesAsync();
    }

    public async Task RemoveGameFromTheCartAsync(Guid customerId, string gameKey)
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

    public async Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrdersAsync()
    {
        var orders =
            (await _orderRepository.GetAllByFilterAsync(o =>
                o.Status == OrderStatus.Paid || o.Status == OrderStatus.Cancelled))
            .Select(o => o.AsDto());

        return orders;
    }

    public async Task<OrderDto?> GetByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.Id == orderId);

        return order?.AsDto();
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetOrderDetailsAsync(Guid orderId)
    {
        var orderGames =
            (await _orderGameRepository.GetAllByFilterAsync(og =>
                og.OrderId == orderId))
            .Select(og => og.AsDto());

        return orderGames;
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetCartAsync(Guid customerId)
    {
        var order = await _orderRepository.GetOneAsync(o =>
            o.CustomerId == customerId && o.Status == OrderStatus.Open)
                    ?? throw new NotFoundException("You do not have products in your cart");

        var orderGames = (await _orderGameRepository.GetAllByFilterAsync(
            og => og.OrderId == order.Id))
            .Select(og => og.AsDto());

        return orderGames;
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.Id == orderId);

        order.Status = OrderStatus.Cancelled;

        await _orderRepository.SaveChangesAsync();
    }

    public async Task PayOrderAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.Id == orderId);

        order.Status = OrderStatus.Paid;

        await _orderRepository.SaveChangesAsync();
    }

    // TODO: Make the invoice prettier
    public async Task<byte[]> GenerateInvoicePdfAsync(Guid customerId)
    {
        var order = await _orderRepository.GetOneAsync(o => o.CustomerId == customerId
                                                            && o.Status == OrderStatus.Open) ??
                    throw new OrderNotFoundException(customerId);

        int invoiceValidityInDays = Convert.ToInt32(configuration.GetSection("InvoiceValidity").Value);

        var sumTotal =
            (await _orderGameRepository.GetAllByFilterAsync(og => og.OrderId == order.Id))
            .Select(og => og.Price * og.Quantity)
            .Sum();

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Header().Text("Invoice");

                page.Content().Column(column =>
                {
                    column.Spacing(20);

                    column.Item().Text($"User ID: {customerId}");
                    column.Item().Text($"Order ID: {order.Id}");
                    column.Item().Text($"Creation date: {DateTime.Now}");
                    column.Item().Text($"Invoice valid until: {DateTime.Now.AddDays(invoiceValidityInDays)}");
                    column.Item().Text($"Sum: ${sumTotal}");
                });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<double> GetCartSumAsync(Guid customerId)
    {
        var order = await GetOrderOrElseThrow(customerId);
        var gamesSum =
            (await _orderGameRepository.GetAllByFilterAsync(og => og.OrderId == order.Id))
            .Select(o => o.Quantity * o.Price)
            .Sum();

        return gamesSum;
    }

    public async Task<Guid> GetCartIdAsync(Guid customerId)
    {
        return (await GetOrderOrElseThrow(customerId)).Id;
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