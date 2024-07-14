using System.Linq.Expressions;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.Services.GameService;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.BLL.Tests.Services;

public class GameServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<GameGenre>> _gameGenreRepositoryMock;
    private readonly Mock<IRepository<GamePlatform>> _gamePlatformRepositoryMock;
    private readonly GameService _service;

    public GameServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _gameGenreRepositoryMock = _fixture.Freeze<Mock<IRepository<GameGenre>>>();
        _gamePlatformRepositoryMock = _fixture.Freeze<Mock<IRepository<GamePlatform>>>();

        _service = new GameService(_gameRepositoryMock.Object, _gameGenreRepositoryMock.Object, _gamePlatformRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsyncReturnsAllGames()
    {
        // Arrange
        var games = _fixture.Create<ICollection<Game>>();
        _gameRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(games);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        _gameRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        result.Should().BeEquivalentTo(games.Select(g => g.AsDto()));
    }

    [Fact]
    public async Task GetByKeyAsyncReturnsGameWhenGameIsFound()
    {
        // Arrange
        var game = _fixture.Create<Game>();
        var key = game.Key;
        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == key)).ReturnsAsync(game);

        // Act
        var result = await _service.GetByKeyAsync(key);

        // Assert
        result.Should().BeEquivalentTo(game.AsDto());
    }

    [Fact]
    public async Task GetByKeyAsyncReturnsNullWhenGameNotFound()
    {
        // Arrange
        string key = "non-existing-key";
        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == key)).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetByKeyAsync(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByGenreAsyncReturnsGames()
    {
        // Arrange
        var genreId = Guid.NewGuid();

        var gameGenre = new List<GameGenre>
        {
            new() { GameId = Guid.NewGuid(), GenreId = genreId },
            new() { GameId = Guid.NewGuid(), GenreId = genreId },
            new() { GameId = Guid.NewGuid(), GenreId = genreId },
        };

        var games = gameGenre.Select(gg => new Game { Id = gg.GameId })
            .ToList();

        _gameGenreRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.GenreId == genreId))
            .ReturnsAsync(gameGenre);
        _gameRepositoryMock
            .Setup(x => x.GetAllByFilterAsync(It.IsAny<Expression<Func<Game, bool>>>()))
            .ReturnsAsync(games);

        // Act
        var result = await _service.GetByGenreAsync(genreId);

        // Assert
        result.Should().BeEquivalentTo(games.Select(g => g.AsDto()));
    }

    [Fact]
    public async Task GetByGenreAsyncReturnsEmpty()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var emptyGameGenreCollection = new List<GameGenre>();

        _gameGenreRepositoryMock
            .Setup(x => x.GetAllByFilterAsync(gg => gg.GenreId == genreId))
            .ReturnsAsync(emptyGameGenreCollection);

        // Act
        var result = await _service.GetByGenreAsync(genreId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByPlatformAsyncReturnsGamesWhenGamesAreFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();

        var gamePlatforms = new List<GamePlatform>
        {
            new() { GameId = Guid.NewGuid(), PlatformId = platformId },
            new() { GameId = Guid.NewGuid(), PlatformId = platformId },
            new() { GameId = Guid.NewGuid(), PlatformId = platformId },
        };

        var games = gamePlatforms.Select(gg => new Game { Id = gg.GameId }).ToList();

        _gamePlatformRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.PlatformId == platformId)).ReturnsAsync(gamePlatforms);
        _gameRepositoryMock.Setup(x => x.GetAllByFilterAsync(It.IsAny<Expression<Func<Game, bool>>>())).ReturnsAsync(games);

        // Act
        var result = await _service.GetByPlatformAsync(platformId);

        // Assert
        result.Should().BeEquivalentTo(games.Select(g => g.AsDto()));
    }

    [Fact]
    public async Task GetByPlatformAsyncReturnsEmptyWhenNoGamesFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();

        _gamePlatformRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.PlatformId == platformId)).ReturnsAsync(new List<GamePlatform>());

        // Act
        var result = await _service.GetByPlatformAsync(platformId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsyncReturnsGameWhenGameIsFound()
    {
        // Arrange
        var game = _fixture.Create<Game>();
        var id = game.Id;
        _gameRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(game);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeEquivalentTo(game.AsDto());
    }

    [Fact]
    public async Task GetByIdAsyncReturnsNullWhenGameIsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _gameRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Game)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsyncUpdatesGameSuccessfully()
    {
        // Arrange
        var game = _fixture.Create<Game>();
        var updateGameDto = _fixture.Create<UpdateGameDto>();

        _gameRepositoryMock.Setup(x => x.GetByIdAsync(updateGameDto.Id)).ReturnsAsync(game);

        // Act
        await _service.UpdateAsync(updateGameDto);

        // Assert
        _gameRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncThrowsExceptionWhenGameNotFound()
    {
        // Arrange
        var updateGameDto = _fixture.Create<UpdateGameDto>();
        _gameRepositoryMock.Setup(x => x.GetByIdAsync(updateGameDto.Id)).ReturnsAsync((Game)null);

        // Act and Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _service.UpdateAsync(updateGameDto));
    }

    [Fact]
    public async Task UpdateAsyncThrowsException()
    {
        // Arrange
        var game = _fixture.Create<Game>();
        var updateGameDto = _fixture.Create<UpdateGameDto>();

        _gameRepositoryMock.Setup(x => x.GetByIdAsync(updateGameDto.Id)).ReturnsAsync(game);
        _gameRepositoryMock.Setup(x => x.SaveChangesAsync()).ThrowsAsync(new Exception());

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(updateGameDto));
    }

    [Fact]
    public async Task CreateAsyncCreatesGameSuccessfully()
    {
        // Arrange
        var createGameDto = _fixture.Create<CreateGameDto>();
        var gameGenres = createGameDto.GenresIds?.Select(genreId => new GameGenre { GenreId = genreId }).ToList() ??
                         [];
        var gamePlatforms = createGameDto.PlatformsIds?.Select(platformId => new GamePlatform { PlatformId = platformId }).ToList() ??
                            [];

        // Act
        await _service.CreateAsync(createGameDto);

        // Assert
        _gameRepositoryMock.Verify(x => x.CreateAsync(It.Is<Game>(g => g.Name == createGameDto.Name && g.Key == createGameDto.Key)), Times.Once);
        foreach (var gameGenre in gameGenres)
        {
            _gameGenreRepositoryMock
                .Verify(
                    x =>
                        x.CreateAsync(It.Is<GameGenre>(gg => gg.GenreId == gameGenre.GenreId)),
                    Times.Once());
        }

        foreach (var gamePlatform in gamePlatforms)
        {
            _gamePlatformRepositoryMock
                .Verify(
                    x =>
                        x.CreateAsync(It.Is<GamePlatform>(gp => gp.PlatformId == gamePlatform.PlatformId)),
                    Times.Once());
        }

        _gameRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteByKeyAsyncDeletesGameSuccessfully()
    {
        // Arrange
        string key = "testKey";

        _gameRepositoryMock.Setup(x => x.DeleteByFilterAsync(g => g.Key == key)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteByKeyAsync(key);

        // Assert
        _gameRepositoryMock.Verify(x => x.DeleteByFilterAsync(g => g.Key == key), Times.Once);
        _gameRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}