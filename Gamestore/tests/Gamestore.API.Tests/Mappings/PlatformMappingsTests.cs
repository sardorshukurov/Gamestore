using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.API.Tests.Mappings;

public class PlatformMappingsTests
{
    private readonly IFixture _fixture;

    public PlatformMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsResponseShouldReturnMappedPlatformResponse()
    {
        // Arrange
        var dto = _fixture.Create<PlatformDto>();
        var games = _fixture.Create<ICollection<GameShortResponse>>();

        // Act
        var response = dto.AsResponse(games);

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Type.Should().Be(dto.Type);
        response.Games.Should().BeEquivalentTo(games);
    }

    [Fact]
    public void AsShortResponseFromDtoShouldReturnMappedPlatformShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<PlatformDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsShortResponseFromShortDtoShouldReturnMappedPlatformShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<PlatformShortDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsCreatePlatformDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<CreatePlatformRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsUpdatePlatformDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }
}