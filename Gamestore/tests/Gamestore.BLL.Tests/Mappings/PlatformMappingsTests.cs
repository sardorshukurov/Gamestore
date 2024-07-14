using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.DAL.Entities;

namespace Gamestore.BLL.Tests.Mappings;

public class PlatformMappingsTests
{
    private readonly IFixture _fixture;

    public PlatformMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsDtoMapsCorrectly()
    {
        // Arrange
        var platform = _fixture.Create<Platform>();
        var games = _fixture.Create<ICollection<GameShortDto>>();

        // Act
        var dto = platform.AsDto(games);

        // Assert
        dto.Id.Should().Be(platform.Id);
        dto.Type.Should().Be(platform.Type);
        dto.Games.Should().BeEquivalentTo(games);
    }

    [Fact]
    public void AsShortDtoMapsCorrectly()
    {
        // Arrange
        var platform = _fixture.Create<Platform>();

        // Act
        var shortDto = platform.AsShortDto();

        // Assert
        shortDto.Id.Should().Be(platform.Id);
        shortDto.Type.Should().Be(platform.Type);
    }

    [Fact]
    public void AsEntityMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<CreatePlatformDto>();

        // Act
        var platform = dto.AsEntity();

        // Assert
        platform.Type.Should().Be(dto.Type);
    }

    [Fact]
    public void UpdateEntityUpdatesCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<UpdatePlatformDto>();
        var platform = _fixture.Create<Platform>();

        // Act
        dto.UpdateEntity(platform);

        // Assert
        platform.Type.Should().Be(dto.Type);
    }
}