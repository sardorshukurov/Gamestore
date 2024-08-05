using System.Linq.Expressions;
using Gamestore.BLL.Services.OrderService;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Gamestore.Tests.BLL.Services;

public class OrderServiceTests
{
    private readonly IFixture _fixture;

    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IRepository<OrderGame>> _orderGameRepositoryMock;

    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _orderRepositoryMock = _fixture.Freeze<Mock<IRepository<Order>>>();
        _orderGameRepositoryMock = _fixture.Freeze<Mock<IRepository<OrderGame>>>();
        var configurationMock = _fixture.Freeze<Mock<IConfiguration>>();

        _service = new OrderService(
            _orderRepositoryMock.Object,
            _orderGameRepositoryMock.Object,
            _gameRepositoryMock.Object,
            configurationMock.Object);
    }

    [Fact]
    public async Task AddGameInTheCartAsyncAddsGame()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        string gameKey = _fixture.Create<string>();
        var game = _fixture.Create<Game>();
        var order = _fixture.Create<Order>();

        game.UnitInStock = 10;

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey))
            .ReturnsAsync(game);
        _orderRepositoryMock.Setup(x => x.GetOneAsync(o => o.Status == OrderStatus.Open
                                                           && o.CustomerId == customerId))
            .ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetOneAsync(og => og.OrderId == order.Id
                                                                && og.ProductId == game.Id))
            .ReturnsAsync((OrderGame)null);

        // Act
        await _service.AddGameInTheCartAsync(customerId, gameKey);

        // Assert
        _orderGameRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<OrderGame>()), Times.Once);
        _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveGameFromTheCartAsyncRemovesGame()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        string gameKey = _fixture.Create<string>();
        var game = _fixture.Create<Game>();
        var order = _fixture.Create<Order>();
        var orderGame = _fixture.Create<OrderGame>();

        game.UnitInStock = 10;

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _orderRepositoryMock.Setup(x => x.GetOneAsync(o => o.Status == OrderStatus.Open && o.CustomerId == customerId)).ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetOneAsync(og => og.OrderId == order.Id && og.ProductId == game.Id)).ReturnsAsync(orderGame);
        _orderGameRepositoryMock.Setup(x => x.DeleteOneAsync(It.IsAny<Expression<Func<OrderGame, bool>>>())).Returns(Task.CompletedTask);

        // Act
        await _service.RemoveGameFromTheCartAsync(customerId, gameKey);

        // Assert
        _orderGameRepositoryMock.Verify(x => x.DeleteOneAsync(It.IsAny<Expression<Func<OrderGame, bool>>>()), Times.Once);
        _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.AtMost(2));
    }

    [Fact]
    public async Task GetPaidAndCancelledOrdersAsyncReturnsCorrectOrders()
    {
        // Arrange
        var orders = _fixture.Create<IEnumerable<Order>>().ToList();
        _orderRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _service.GetPaidAndCancelledOrdersAsync();

        // Assert
        Assert.Equal(orders.Count, result.Count());
        _orderRepositoryMock.Verify(
            x => x.GetAllByFilterAsync(
            It.IsAny<Expression<Func<Order, bool>>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsyncReturnsCorrectOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = _fixture.Create<Order>();

        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);

        // Act
        var result = await _service.GetByIdAsync(orderId);

        // Assert
        Assert.Equal(order.Id, result.Id);
        _orderRepositoryMock.Verify(
            x => x.GetOneAsync(
            It.IsAny<Expression<Func<Order, bool>>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetOrderDetailsAsyncReturnsCorrectOrderDetails()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderGames = _fixture.Create<IEnumerable<OrderGame>>().ToList();

        _orderGameRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<OrderGame, bool>>>()))
            .ReturnsAsync(orderGames);

        // Act
        var result = await _service.GetOrderDetailsAsync(orderId);

        // Assert
        Assert.Equal(orderGames.Count, result.Count());
        _orderGameRepositoryMock.Verify(
            x => x.GetAllByFilterAsync(
            It.IsAny<Expression<Func<OrderGame, bool>>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCartAsyncReturnsCorrectCart()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = _fixture.Create<Order>();
        var orderGames = _fixture.Create<IEnumerable<OrderGame>>().ToList();

        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<OrderGame, bool>>>()))
            .ReturnsAsync(orderGames);

        // Act
        var result = await _service.GetCartAsync(customerId);

        // Assert
        Assert.Equal(orderGames.Count, result.Count());
        _orderRepositoryMock.Verify(
            x => x.GetOneAsync(
            It.IsAny<Expression<Func<Order, bool>>>()),
            Times.Once);
        _orderGameRepositoryMock.Verify(
            x => x.GetAllByFilterAsync(
            It.IsAny<Expression<Func<OrderGame, bool>>>()),
            Times.Once);
    }

    [Fact]
    public async Task CancelOrderAsyncCancelsOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = _fixture.Create<Order>();
        var orderGames = _fixture.Create<List<OrderGame>>();
        var game = _fixture.Create<Game>();

        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<OrderGame, bool>>>()))
            .ReturnsAsync(orderGames);
        _gameRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(game);

        // Act
        await _service.CancelOrderAsync(orderId);

        // Assert
        Assert.Equal(OrderStatus.Cancelled, order.Status);
        _orderRepositoryMock.Verify(
            x => x.GetOneAsync(
            It.IsAny<Expression<Func<Order, bool>>>()),
            Times.Once);
        _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PayOrderAsyncSetsOrderToPaid()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);

        // Act
        await _service.PayOrderAsync(order.Id);

        // Assert
        Assert.Equal(OrderStatus.Paid, order.Status);
        _orderRepositoryMock.Verify(
            x => x.GetOneAsync(
            It.IsAny<Expression<Func<Order, bool>>>()),
            Times.Once);
        _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GenerateInvoicePdfAsyncGeneratesInvoice()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = _fixture.Create<Order>();
        var orderGames = _fixture.Create<List<OrderGame>>();

        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<OrderGame, bool>>>()))
            .ReturnsAsync(orderGames);

        // Act
        var result = await _service.GenerateInvoicePdfAsync(customerId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCartSumAsyncReturnsCorrectSum()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = _fixture.Create<Order>();
        var orderGames = _fixture.Create<List<OrderGame>>();

        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);
        _orderGameRepositoryMock.Setup(x => x.GetAllByFilterAsync(
                It.IsAny<Expression<Func<OrderGame, bool>>>()))
            .ReturnsAsync(orderGames);

        // Act
        var result = await _service.GetCartSumAsync(customerId);

        // Assert
        var expectedSum = orderGames.Select(og => og.Price * ((100 - og.Discount) / 100.0) * og.Quantity).Sum();
        Assert.Equal(expectedSum, result);
    }

    [Fact]
    public async Task GetCartIdAsyncReturnsCorrectId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = _fixture.Create<Order>();
        _orderRepositoryMock.Setup(x => x.GetOneAsync(
                It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(order);

        // Act
        var result = await _service.GetCartIdAsync(customerId);

        // Assert
        Assert.Equal(order.Id, result);
    }
}