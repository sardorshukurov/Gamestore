using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Genre;
using Gamestore.DAL.Entities;

namespace Gamestore.BLL.Tests.Mappings;

public class GenreMappingsTests
{
    private readonly IFixture _fixture;

    public GenreMappingsTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void AsDtoMapsCorrectly()
    {
        // Arrange
        var genre = _fixture.Create<Genre>();
        var parentGenre = _fixture.Create<GenreShortDto>();
        var games = _fixture.Create<ICollection<GameShortDto>>();

        // Act
        var dto = genre.AsDto(games, parentGenre);

        // Assert
        dto.Id.Should().Be(genre.Id);
        dto.Name.Should().Be(genre.Name);
        dto.ParentGenreId.Should().Be(parentGenre.Id);
        dto.ParentGenreName.Should().Be(parentGenre.Name);
        dto.Games.Should().BeEquivalentTo(games);
    }

    [Fact]
    public void AsShortDtoMapsCorrectly()
    {
        // Arrange
        var genre = _fixture.Create<Genre>();

        // Act
        var shortDto = genre.AsShortDto();

        // Assert
        shortDto.Id.Should().Be(genre.Id);
        shortDto.Name.Should().Be(genre.Name);
        shortDto.ParentGenreId.Should().Be(genre.ParentGenreId);
    }

    [Fact]
    public void AsEntityMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<CreateGenreDto>();

        // Act
        var genre = dto.AsEntity();

        // Assert
        genre.Name.Should().Be(dto.Name);
        genre.ParentGenreId.Should().Be(dto.ParentGenreId);
    }

    [Fact]
    public void UpdateEntityUpdatesCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<UpdateGenreDto>();
        var genre = _fixture.Create<Genre>();

        // Act
        dto.UpdateEntity(genre);

        // Assert
        genre.Name.Should().Be(dto.Name);
        genre.ParentGenreId.Should().Be(dto.ParentGenreId);
    }
}