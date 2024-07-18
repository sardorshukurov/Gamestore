using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.DTOs.Genre;

namespace Gamestore.Tests.API.Mappings;

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
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
        response.ParentGenreId.Should().Be(dto.ParentGenreId);
    }

    [Fact]
    public void AsShortResponsFromDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GenreDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
    }

    [Fact]
    public void AsShortResponseFromShortDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<GenreShortDto>();

        // Act
        var response = dto.AsShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
    }

    [Fact]
    public void AsCreateGenreDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateGenreRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        request.Genre.Name.Should().Be(dto.Name);
        request.Genre.ParentGenreId.Should().Be(dto.ParentGenreId);
    }

    [Fact]
    public void AsUpdateGenreDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGenreRequest>();

        // Act
        var dto = request.AsDto();

        // Assert
        request.Genre.Id.Should().Be(dto.Id);
        request.Genre.Name.Should().Be(dto.Name);
        request.Genre.ParentGenreId.Should().Be(dto.ParentGenreId);
    }
}