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
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsShortResponseShouldReturnMappedGameShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GameDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsCreateGameDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateGameRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsUpdateGameDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }
}