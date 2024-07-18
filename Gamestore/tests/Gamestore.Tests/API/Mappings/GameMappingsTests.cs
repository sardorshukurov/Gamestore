using Gamestore.API.DTOs.Game;
using Gamestore.BLL.DTOs.Game;

namespace Gamestore.Tests.API.Mappings;

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
        var dto = _fixture.Create<GameDto>();

        // Act
        var response = dto.AsResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
        response.Key.Should().Be(dto.Key);
        response.Description.Should().Be(dto.Description);
        response.Price.Should().Be(dto.Price);
        response.Discount.Should().Be(dto.Discount);
        response.UnitInStock.Should().Be(dto.UnitInStock);
    }

    [Fact]
    public void AsShortResponseShouldReturnMappedGameShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GameDto>();

        // Act
        var response = dto.AsShortResponse();

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
        var dto = request.AsDto();

        // Assert
        request.Game.Name.Should().Be(dto.Name);
        request.Game.Key.Should().Be(dto.Key);
        request.Game.Description.Should().Be(dto.Description);
        request.Game.Price.Should().Be(dto.Price);
        request.Game.Discount.Should().Be(dto.Discount);
        request.Game.UnitInStock.Should().Be(dto.UnitInStock);
        request.Publisher.Should().Be(dto.PublisherId);
        request.Platforms.Should().BeEquivalentTo(dto.PlatformsIds);
        request.Genres.Should().BeEquivalentTo(dto.GenresIds);
    }

    [Fact]
    public void AsUpdateGameDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        request.Game.Name.Should().Be(dto.Name);
        request.Game.Key.Should().Be(dto.Key);
        request.Game.Description.Should().Be(dto.Description);
        request.Game.Price.Should().Be(dto.Price);
        request.Game.Discount.Should().Be(dto.Discount);
        request.Game.UnitInStock.Should().Be(dto.UnitInStock);
        request.Publisher.Should().Be(dto.PublisherId);
        request.Platforms.Should().BeEquivalentTo(dto.PlatformsIds);
        request.Genres.Should().BeEquivalentTo(dto.GenresIds);
    }
}