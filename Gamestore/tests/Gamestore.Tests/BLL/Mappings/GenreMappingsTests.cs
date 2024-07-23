using Gamestore.BLL.DTOs.Genre;
using Gamestore.Domain.Entities;

namespace Gamestore.Tests.BLL.Mappings;

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
        var dto = _fixture.Create<Genre>();

        // Act
        var response = dto.ToResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
        response.ParentGenreId.Should().Be(dto.ParentGenreId);
    }

    [Fact]
    public void AsShortResponsFromDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<Genre>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.Name.Should().Be(dto.Name);
    }

    [Fact]
    public void AsShortResponseFromShortDtoShouldReturnMappedGenreShortResponse()
    {
        // Arrange
        var dto = _fixture.Create<Genre>();

        // Act
        var response = dto.ToShortResponse();

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
        var entity = request.ToEntity();

        // Assert
        request.Genre.Name.Should().Be(entity.Name);
        request.Genre.ParentGenreId.Should().Be(entity.ParentGenreId);
    }

    [Fact]
    public void AsUpdateGenreDtoShouldReturnMappedDtoFromRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGenreRequest>();

        var entity = new Genre();

        // Act
        request.UpdateEntity(entity);

        // Assert
        request.Genre.Id.Should().Be(entity.Id);
        request.Genre.Name.Should().Be(entity.Name);
        request.Genre.ParentGenreId.Should().Be(entity.ParentGenreId);
    }
}