using System.Linq.Expressions;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.Tests.BLL.Services;

public class PlatformServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<GamePlatform>> _gamePlatformRepositoryMock;
    private readonly Mock<IRepository<Platform>> _platformRepostioryMock;
    private readonly PlatformService _service;

    public PlatformServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _gamePlatformRepositoryMock = _fixture.Freeze<Mock<IRepository<GamePlatform>>>();
        _platformRepostioryMock = _fixture.Freeze<Mock<IRepository<Platform>>>();

        _service = new PlatformService(_platformRepostioryMock.Object, _gameRepositoryMock.Object, _gamePlatformRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsyncReturnsPlatforms()
    {
        // Arrange
        var platforms = new List<Platform>
        {
            new() { Id = Guid.NewGuid(), Type = "Type1" },
            new() { Id = Guid.NewGuid(), Type = "Type2" },
            new() { Id = Guid.NewGuid(), Type = "Type3" },
        };

        _platformRepostioryMock
            .Setup(g => g.GetAllAsync())
            .ReturnsAsync(platforms);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(platforms.Select(p => p.ToShortDto()));
    }

    [Fact]
    public async Task GetByIdAsyncReturnsPlatformWhenPlatformIsFound()
    {
        // Arrange
        var platform = _fixture.Create<Platform>();
        var id = platform.Id;

        _platformRepostioryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(platform);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeEquivalentTo(platform.ToShortDto());
    }

    [Fact]
    public async Task GetByIdAsyncReturnsNullWhenPlatformIsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _platformRepostioryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((Platform)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsyncUpdatesPlatformSuccessfully()
    {
        // Arrange
        var platformToUpdate = _fixture.Create<Platform>();
        var updatePlatformDto = _fixture.Create<UpdatePlatformDto>();

        _platformRepostioryMock.Setup(x => x.GetByIdAsync(updatePlatformDto.Id)).ReturnsAsync(platformToUpdate);

        // Act
        await _service.UpdateAsync(updatePlatformDto);

        // Assert
        _platformRepostioryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncThrowsExceptionWhenPlatformNotFound()
    {
        // Arrange
        var updatePlatformDto = _fixture.Create<UpdatePlatformDto>();
        _platformRepostioryMock.Setup(x => x.GetByIdAsync(updatePlatformDto.Id)).ReturnsAsync((Platform)null);

        // Act and Assert
        await Assert.ThrowsAsync<PlatformNotFoundException>(() => _service.UpdateAsync(updatePlatformDto));
    }

    [Fact]
    public async Task DeleteAsyncDeletesPlatformSuccessfully()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _platformRepostioryMock.Setup(x => x.DeleteByIdAsync(id)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _platformRepostioryMock.Verify(x => x.DeleteByIdAsync(id), Times.Once);
        _platformRepostioryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsyncCreatesPlatformSuccessfully()
    {
        // Arrange
        var createPlatformDto = _fixture.Create<CreatePlatformDto>();

        // Act
        await _service.CreateAsync(createPlatformDto);

        // Assert
        _platformRepostioryMock.Verify(x => x.CreateAsync(It.Is<Platform>(p => p.Type == createPlatformDto.Type)), Times.Once);
        _platformRepostioryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllByGameKeyAsyncReturnsPlatformsWhenPlatformsAreFound()
    {
        // Arrange
        string gameKey = "testGameKey";
        var game = _fixture.Create<Game>();
        game.Key = gameKey;
        var platformIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var platforms = platformIds.Select(id => new Platform { Id = id }).ToList();

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _gamePlatformRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.GameId == game.Id)).ReturnsAsync(platformIds.Select(id => new GamePlatform { PlatformId = id }).ToList());
        _platformRepostioryMock.Setup(x => x.GetAllByFilterAsync(It.IsAny<Expression<Func<Platform, bool>>>())).ReturnsAsync(platforms);

        // Act
        var result = await _service.GetAllByGameKeyAsync(gameKey);

        // Assert
        result.Should().BeEquivalentTo(platforms.Select(p => p.ToShortDto()));
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
    public async Task GetAllByGameKeyAsyncReturnsEmptyWhenNoPlatformsAreFound()
    {
        // Arrange
        string gameKey = "testGameKey";
        var game = _fixture.Create<Game>();
        game.Key = gameKey;

        _gameRepositoryMock.Setup(x => x.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _gamePlatformRepositoryMock.Setup(x => x.GetAllByFilterAsync(gg => gg.GameId == game.Id)).ReturnsAsync([]);
        _platformRepostioryMock.Setup(x => x.GetAllByFilterAsync(p => It.IsAny<IEnumerable<Guid>>().Contains(p.Id))).ReturnsAsync([]);

        // Act
        var result = await _service.GetAllByGameKeyAsync(gameKey);

        // Assert
        result.Should().BeEmpty();
    }
}