using Gamestore.BLL.DTOs.Order;
using Gamestore.BLL.DTOs.Order.Payment;
using Gamestore.Common.Exceptions.BadRequest;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Gamestore.BLL.Services.OrderService;

public class OrderService(
    IRepository<Order> orderRepository,
    IRepository<OrderGame> orderGameRepository,
    IRepository<Game> gameRepository,
    IRepository<PaymentMethod> paymentMethodRepository,
    IConfiguration configuration) : IOrderService
{
    public async Task AddGameInTheCartAsync(Guid customerId, string gameKey)
    {
        var game = await GetGameOrElseThrow(gameKey);
        EnsureGameIsInStock(game);

        var order = await GetOrCreateOpenOrder(customerId);

        await AddGameToOrder(game, order);
        game.UnitInStock--;

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

        await orderRepository.SaveChangesAsync();

        await DeleteOrderIfNoGamesLeft(order.Id);
    }

    public async Task<IEnumerable<OrderResponse>> GetPaidAndCancelledOrdersAsync()
    {
        var orders =
            (await orderRepository.GetAllByFilterAsync(o =>
                o.Status == OrderStatus.Paid || o.Status == OrderStatus.Cancelled))
            .Select(o => o.ToResponse());

        return orders;
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersHistoryAsync(DateTime start, DateTime end)
    {
        var orders = (await orderRepository.GetAllByFilterAsync(
                o => o.Date >= start && o.Date <= end))
            .Select(o => o.ToResponse());

        return orders;
    }

    public async Task<OrderResponse?> GetByIdAsync(Guid orderId)
    {
        var order = await orderRepository.GetOneAsync(o => o.Id == orderId);

        return order?.ToResponse();
    }

    public async Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsAsync(Guid orderId)
    {
        var orderGames =
            (await orderGameRepository.GetAllByFilterAsync(og =>
                og.OrderId == orderId))
            .Select(og => og.ToResponse());

        return orderGames;
    }

    public async Task<IEnumerable<OrderDetailsResponse>> GetCartAsync(Guid customerId)
    {
        var order = await GetOpenOrderByCustomerIdOrElseThrow(customerId);

        var orderGames = (await orderGameRepository.GetAllByFilterAsync(
            og => og.OrderId == order.Id))
            .Select(og => og.ToResponse());

        return orderGames;
    }

    public async Task CancelOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetOneAsync(o => o.Id == orderId);

        order.Status = OrderStatus.Cancelled;

        var orderGames = await orderGameRepository.GetAllByFilterAsync(og => og.OrderId == orderId);

        // adds back quantity of taken games to the stock
        foreach (var orderGame in orderGames)
        {
            var game = await gameRepository.GetByIdAsync(orderGame.ProductId);

            if (game is not null)
            {
                game.UnitInStock += orderGame.Quantity;
            }
        }

        await orderRepository.SaveChangesAsync();
    }

    public async Task PayOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetOneAsync(o => o.Id == orderId);

        order.Status = OrderStatus.Paid;

        await orderRepository.SaveChangesAsync();
    }

    // TODO: Make the invoice prettier (someday I will :P)
    public async Task<byte[]> GenerateInvoicePdfAsync(Guid customerId)
    {
        var order = await GetOpenOrderByCustomerIdOrElseThrow(customerId);

        // gets invoice validity in days from app settings
        int invoiceValidityInDays = Convert.ToInt32(configuration.GetSection("InvoiceValidity").Value);

        // calculates total sum of the products
        var sumTotal =
            (await orderGameRepository.GetAllByFilterAsync(og => og.OrderId == order.Id))
            .Select(og => og.Price * ((100 - og.Discount) / 100.0) * og.Quantity)
            .Sum();

        // TODO: probably move this to somewhere else
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
        var order = await GetOpenOrderByCustomerIdOrElseThrow(customerId);
        var gamesSum =
            (await orderGameRepository.GetAllByFilterAsync(og => og.OrderId == order.Id))
            .Select(og => og.Price * ((100 - og.Discount) / 100.0) * og.Quantity)
            .Sum();

        return gamesSum;
    }

    public async Task<Guid> GetCartIdAsync(Guid customerId)
    {
        return (await GetOpenOrderByCustomerIdOrElseThrow(customerId)).Id;
    }

    public async Task<IEnumerable<PaymentMethodResponse>> GetPaymentMethodsAsync()
    {
        var paymentMethods = (await paymentMethodRepository.GetAllAsync())
            .Select(pm => pm.ToResponse());

        return paymentMethods;
    }

    /// <summary>
    /// Gets the game by game key or throws an exception.
    /// </summary>
    /// <param name="gameKey">The game key.</param>
    /// <returns>Game.</returns>
    /// <exception cref="GameNotFoundException">Exception is thrown when the game is not found.</exception>
    private async Task<Game> GetGameOrElseThrow(string gameKey)
    {
        return await gameRepository.GetOneAsync(g => g.Key == gameKey) ?? throw new GameNotFoundException(gameKey);
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
        var orderGame = await orderGameRepository.GetOneAsync(og => og.OrderId == order.Id && og.ProductId == game.Id);

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