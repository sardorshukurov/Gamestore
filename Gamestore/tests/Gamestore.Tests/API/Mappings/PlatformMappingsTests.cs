using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.Tests.API.Mappings;

public class PlatformMappingsTests
{
    private readonly IFixture _fixture;

    public PlatformMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsResponseMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<PlatformDto>();
        var games = _fixture.Create<ICollection<GameShortResponse>>();

        // Act
        var response = dto.ToResponse(games);

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Type.Should().Be(dto.Type);
        response.Games.Should().BeEquivalentTo(games);
    }

    [Fact]
    public void AsShortResponseFromDtoMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<PlatformDto>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Type.Should().Be(dto.Type);
    }

    [Fact]
    public void AsShortResponseFromShortDtoMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<PlatformShortDto>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Type.Should().Be(dto.Type);
    }

    [Fact]
    public void AsCreatePlatformDtoMapsCorrectly()
    {
        // Arrange
        var request = _fixture.Create<CreatePlatformRequest>();

        // Act
        var dto = request.ToDto();

        // Assert
        dto.Type.Should().Be(request.Platform.Type);
    }

    [Fact]
    public void AsUpdatePlatformDtoMapsCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        // Act
        var dto = request.ToDto();

        // Assert
        dto.Id.Should().Be(request.Platform.Id);
        dto.Type.Should().Be(request.Platform.Type);
    }
}