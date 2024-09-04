using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.Domain.Entities.Games;

namespace Gamestore.Tests.BLL.Mappings;

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
        var dto = _fixture.Create<Platform>();
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
        var dto = _fixture.Create<Platform>();

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
        var dto = _fixture.Create<Platform>();

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
        var entity = request.ToEntity();

        // Assert
        entity.Type.Should().Be(request.Type);
    }

    [Fact]
    public void AsUpdatePlatformDtoMapsCorrectly()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        var entity = new Platform();

        // Act
        request.UpdateEntity(entity);

        // Assert
        entity.Id.Should().Be(request.Id);
        entity.Type.Should().Be(request.Type);
    }
}