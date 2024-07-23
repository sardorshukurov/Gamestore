using Gamestore.BLL.DTOs.Game;
using Gamestore.DAL.Entities;

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
    public void AsDtoMapsCorrectly()
    {
        // Arrange
        var game = _fixture.Create<Game>();

        // Act
        var dto = game.ToDto();

        // Assert
        dto.Id.Should().Be(game.Id);
        dto.Name.Should().Be(game.Name);
        dto.Key.Should().Be(game.Key);
        dto.Description.Should().Be(game.Description);
    }

    [Fact]
    public void AsShortDtoMapsCorrectly()
    {
        // Arrange
        var game = _fixture.Create<Game>();

        // Act
        var shortDto = game.ToShortDto();

        // Assert
        shortDto.Id.Should().Be(game.Id);
        shortDto.Name.Should().Be(game.Name);
    }

    [Fact]
    public void AsEntityMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<CreateGameDto>();

        // Act
        var game = dto.ToEntity();

        // Assert
        game.Name.Should().Be(dto.Name);
        game.Key.Should().Be(dto.Key ?? dto.Name);
        game.Description.Should().Be(dto.Description);
    }

    [Fact]
    public void UpdateEntityUpdatesCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<UpdateGameDto>();
        var game = _fixture.Create<Game>();

        // Act
        dto.UpdateEntity(game);

        // Assert
        game.Name.Should().Be(dto.Name);
        game.Key.Should().Be(dto.Key ?? dto.Name);
        game.Description.Should().Be(dto.Description);
    }
}