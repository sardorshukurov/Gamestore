using System.Linq.Expressions;
using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.Services.GenreService;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.Tests.BLL.Services;

public class GenreServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<GameGenre>> _gameGenreRepositoryMock;
    private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
    private readonly GenreService _service;

    public GenreServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _gameGenreRepositoryMock = _fixture.Freeze<Mock<IRepository<GameGenre>>>();
        _genreRepositoryMock = _fixture.Freeze<Mock<IRepository<Genre>>>();

        _service = new GenreService(_genreRepositoryMock.Object, _gameGenreRepositoryMock.Object, _gameRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsyncReturnsGenres()
    {
        // Arrange
        var genres = new List<Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "Genre1" },
            new() { Id = Guid.NewGuid(), Name = "Genre2" },
            new() { Id = Guid.NewGuid(), Name = "Genre3" },
        };

        _genreRepositoryMock
            .Setup(g => g.GetAllAsync())
            .ReturnsAsync(genres);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(genres.Select(g => g.ToShortDto()));
    }

    [Fact]
    public async Task GetByIdAsyncReturnsGenreWhenGenreIsFound()
    {
        // Arrange
        var genre = _fixture.Create<Genre>();
        var id = genre.Id;
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(genre);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeEquivalentTo(genre.ToShortDto());
    }

    [Fact]
    public async Task GetByIdAsyncReturnsNullWhenGenreIsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Genre)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSubGenresAsyncReturnsSubGenresWhenSubGenresAreFound()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var subGenres = new List<Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "SubGenre1", ParentGenreId = parentId },
            new() { Id = Guid.NewGuid(), Name = "SubGenre2", ParentGenreId = parentId },
            new() { Id = Guid.NewGuid(), Name = "SubGenre3", ParentGenreId = parentId },
        };

        _genreRepositoryMock.Setup(x => x.GetAllByFilterAsync(g => g.ParentGenreId == parentId)).ReturnsAsync(subGenres);

        // Act
        var result = await _service.GetSubGenresAsync(parentId);

        // Assert
        result.Should().BeEquivalentTo(subGenres.Select(g => g.ToShortDto()));
    }

    [Fact]
    public async Task GetSubGenresAsyncReturnsEmptyWhenNoSubGenresAreFound()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        _genreRepositoryMock.Setup(x => x.GetAllByFilterAsync(g => g.ParentGenreId == parentId)).ReturnsAsync([]);

        // Act
        var result = await _service.GetSubGenresAsync(parentId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsyncUpdatesGenreSuccessfully()
    {
        // Arrange
        var genre = _fixture.Create<Genre>();
        var updateGenreDto = _fixture.Create<UpdateGenreDto>();

        _genreRepositoryMock.Setup(x => x.GetByIdAsync(updateGenreDto.Id)).ReturnsAsync(genre);

        // Act
        await _service.UpdateAsync(updateGenreDto);

        // Assert
        _genreRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncThrowsExceptionWhenGenreNotFound()
    {
        // Arrange
        var updateGenreDto = _fixture.Create<UpdateGenreDto>();
        _genreRepositoryMock.Setup(x => x.GetByIdAsync(updateGenreDto.Id)).ReturnsAsync((Genre)null);

        // Act and Assert
        await Assert.ThrowsAsync<GenreNotFoundException>(() => _service.UpdateAsync(updateGenreDto));
    }

    [Fact]
    public async Task UpdateAsyncThrowsException()
    {
        // Arrange
        var genre = _fixture.Create<Genre>();
        var updateGenreDto = _fixture.Create<UpdateGenreDto>();

        _genreRepositoryMock.Setup(x => x.GetByIdAsync(updateGenreDto.Id)).ReturnsAsync(genre);
        _genreRepositoryMock.Setup(x => x.SaveChangesAsync()).ThrowsAsync(new Exception());

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(updateGenreDto));
    }

    [Fact]
    public async Task DeleteAsyncDeletesGenreSuccessfully()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _genreRepositoryMock.Setup(x => x.DeleteByIdAsync(id)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _genreRepositoryMock.Verify(x => x.DeleteByIdAsync(id), Times.Once);
        _genreRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsyncCreatesGenreSuccessfully()
    {
        // Arrange
        var createGenreDto = _fixture.Create<CreateGenreDto>();
        var genre = createGenreDto.ToEntity();

        // Act
        await _service.CreateAsync(createGenreDto);

        // Assert
        _genreRepositoryMock.Verify(
            x => x.CreateAsync(It.Is<Genre>(g => g.Name == genre.Name &&
                                                                               g.ParentGenreId == genre.ParentGenreId)),
            Times.Once);
        _genreRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsyncThrowsExceptionWhenParentGenreNotFound()
    {
        // Arrange
        var createGenreDto = _fixture.Create<CreateGenreDto>();

        _genreRepositoryMock.Setup(x => x.GetByIdAsync(createGenreDto.ParentGenreId!.Value))
            .ReturnsAsync((Genre)null);

        // Assert
        await Assert.ThrowsAsync<GenreNotFoundException>(() => _service.CreateAsync(createGenreDto));
    }

    [Fact]
    public async Task GetAllByGameKeyAsyncReturnsGenresWhenGenresAreFound()
    {
        // Arrange
        string gameKey = "testGameKey";
        var game = _fixture.Create<Game>();
        game.Key = gameKey;
        var genreIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var genres = genreIds.Select(id => new Genre { Id = id }).ToList();

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _gameGenreRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.GameId == game.Id)).ReturnsAsync(genreIds.Select(id => new GameGenre { GenreId = id }).ToList());
        _genreRepositoryMock.Setup(x => x.GetAllByFilterAsync(It.IsAny<Expression<Func<Genre, bool>>>())).ReturnsAsync(genres);

        // Act
        var result = await _service.GetAllByGameKeyAsync(gameKey);

        // Assert
        result.Should().BeEquivalentTo(genres.Select(g => g.ToShortDto()));
    }

    [Fact]
    public async Task GetAllByGameKeyAsyncThrowsGameNotFoundExceptionWhenGameIsNotFound()
    {
        // Arrange
        string gameKey = "testGameKey";

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync((Game)null);

        // Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _service.GetAllByGameKeyAsync(gameKey));
    }

    [Fact]
    public async Task GetAllByGameKeyAsyncReturnsEmptyWhenNoGenresAreFound()
    {
        // Arrange
        string gameKey = "testGameKey";
        var game = _fixture.Create<Game>();
        game.Key = gameKey;

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _gameGenreRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.GameId == game.Id)).ReturnsAsync([]);
        _genreRepositoryMock.Setup(x => x.GetAllByFilterAsync(g => It.IsAny<IEnumerable<Guid>>().Contains(g.Id))).ReturnsAsync([]);

        // Act
        var result = await _service.GetAllByGameKeyAsync(gameKey);

        // Assert
        result.Should().BeEmpty();
    }
}