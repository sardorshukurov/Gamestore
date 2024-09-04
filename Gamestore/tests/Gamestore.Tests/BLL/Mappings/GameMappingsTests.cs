using Gamestore.BLL.DTOs.Game;
using Gamestore.Domain.Entities.Games;

namespace Gamestore.Tests.BLL.Mappings;

public class GameMappingsTests
{
    private readonly IFixture _fixture;

    public GameMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsResponseShouldReturnMappedGameResponse()
    {
        // Arrange
        var entity = _fixture.Create<Game>();

        // Act
        var response = entity.ToResponse();

        // Assert
        response.Id.Should().Be(entity.Id);
        response.Name.Should().Be(entity.Name);
        response.Key.Should().Be(entity.Key);
        response.Description.Should().Be(entity.Description);
        response.Price.Should().Be(entity.Price);
        response.Discount.Should().Be(entity.Discount);
        response.UnitInStock.Should().Be(entity.UnitInStock);
    }

    [Fact]
    public void AsShortResponseShouldReturnMappedGameShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<Game>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
    }

    [Fact]
    public void AsCreateGameDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateGameRequest>();

        // Act
        var entity = request.ToEntity();

        // Assert
        request.Game.Name.Should().Be(entity.Name);
        request.Game.Key.Should().Be(entity.Key);
        request.Game.Description.Should().Be(entity.Description);
        request.Game.Price.Should().Be(entity.Price);
        request.Game.Discount.Should().Be(entity.Discount);
        request.Game.UnitInStock.Should().Be(entity.UnitInStock);
        request.Publisher.Should().Be(entity.PublisherId);
    }

    [Fact]
    public void AsUpdateGameDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        var entity = new Game();

        // Act
        request.UpdateEntity(entity);

        // Assert
        request.Game.Name.Should().Be(entity.Name);
        request.Game.Key.Should().Be(entity.Key);
        request.Game.Description.Should().Be(entity.Description);
        request.Game.Price.Should().Be(entity.Price);
        request.Game.Discount.Should().Be(entity.Discount);
        request.Game.UnitInStock.Should().Be(entity.UnitInStock);
        request.Publisher.Should().Be(entity.PublisherId);
    }
}