using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.API.Tests.Mappings;

public class GenreMappingsTests
{
    private readonly IFixture _fixture;

    public GenreMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsResponseShouldReturnMappedGenreResponse()
    {
        // Arrange
        var dto = _fixture.Create<GenreDto>();

        // Act
        var response = dto.AsResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsShortResponsFromDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GenreDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsShortResponseFromShortDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GenreShortDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsCreateGenreDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateGenreRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void AsUpdateGenreDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGenreRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        dto.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers());
    }
}