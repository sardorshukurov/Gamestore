using Gamestore.BLL.DTOs.Order;
using Gamestore.DAL.Entities;

namespace Gamestore.Tests.BLL.Mappings;

public class OrderMappingsTests
{
    private readonly IFixture _fixture;

    public OrderMappingsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AsOrderDtoMapsCorrectly()
    {
        // Arrange
        var order = _fixture.Create<Order>();

        // Act
        var dto = order.AsDto();

        // Assert
        dto.Id.Should().Be(order.Id);
        dto.CustomerId.Should().Be(order.CustomerId);
        dto.Date.Should().Be(order.Date);
    }

    [Fact]
    public void AsOrderGameDtoMapsCorrectly()
    {
        // Arrange
        var orderGame = _fixture.Create<OrderGame>();

        // Act
        var dto = orderGame.AsDto();

        // Assert
        dto.ProductId.Should().Be(orderGame.ProductId);
        dto.Price.Should().Be(orderGame.Price);
        dto.Quantity.Should().Be(orderGame.Quantity);
        dto.Discount.Should().Be(orderGame.Discount);
    }
}