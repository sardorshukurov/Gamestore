using System.Text;
using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.OrderService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.Tests.API.Controllers;

public class GamesControllerTests
{
    private readonly IFixture _fixture;

    private readonly Mock<IGameService> _gameServiceMock;
    private readonly Mock<IGenreService> _genreServiceMock;
    private readonly Mock<IPlatformService> _platformServiceMock;
    private readonly Mock<IPublisherService> _publisherServiceMock;
    private readonly Mock<IOrderService> _orderServiceMock;

    private readonly Mock<CreateGameValidator> _createValidator;
    private readonly Mock<UpdateGameValidator> _updateValidator;

    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _gameServiceMock = _fixture.Freeze<Mock<IGameService>>();
        _genreServiceMock = _fixture.Freeze<Mock<IGenreService>>();
        _platformServiceMock = _fixture.Freeze<Mock<IPlatformService>>();
        _publisherServiceMock = _fixture.Freeze<Mock<IPublisherService>>();
        _orderServiceMock = _fixture.Freeze<Mock<IOrderService>>();

        _createValidator = _fixture.Freeze<Mock<CreateGameValidator>>();
        _updateValidator = _fixture.Freeze<Mock<UpdateGameValidator>>();

        _controller = new GamesController(
            _gameServiceMock.Object,
            _genreServiceMock.Object,
            _platformServiceMock.Object,
            _publisherServiceMock.Object,
            _createValidator.Object,
            _updateValidator.Object,
            _orderServiceMock.Object);
    }

    [Fact]
    public async Task GetAllShouldReturnOkResponse()
    {
        // Arrange
        var gamesDtoMock = _fixture.Create<ICollection<GameDto>>();

        _gameServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(gamesDtoMock);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<OkObjectResult>();

        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().NotBeNull();

        var gamesResponse = okResult.Value as IEnumerable<GameResponse>;
        gamesResponse.Should().NotBeNull();
        gamesResponse.Should().HaveCount(gamesDtoMock.Count);

        _gameServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllGamesByPlatformReturnsOkWhenGamesAreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var games = _fixture.Create<ICollection<GameDto>>();
        _gameServiceMock.Setup(x => x.GetByPlatformAsync(id)).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGamesByPlatform(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(games.Select(g => g.ToResponse()));
    }

    [Fact]
    public async Task GetAllGamesByGenreReturnsOkWhenGamesAreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var games = _fixture.Create<ICollection<GameDto>>();
        _gameServiceMock.Setup(x => x.GetByGenreAsync(id)).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGamesByGenre(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(games.Select(g => g.ToResponse()));
    }

    [Fact]
    public async Task GetByKeyShouldReturnOkResponseWhenGameFound()
    {
        // Arrange
        var gameDtoMock = _fixture.Create<GameDto>();
        var gameResponseMock = _fixture.Create<GameResponse>();

        string key = gameDtoMock.Key;

        _gameServiceMock.Setup(x => x.GetByKeyAsync(key)).ReturnsAsync(gameDtoMock);

        // Act
        var result = await _controller.GetByKey(key);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult<GameResponse>>();
        result.Result.Should().BeAssignableTo<OkObjectResult>();
        result.Result.As<OkObjectResult>().Value
            .Should()
            .NotBeNull()
            .And.BeOfType(gameResponseMock.GetType());
        _gameServiceMock.Verify(x => x.GetByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task GetByKeyShouldReturnNotFoundResponseWhenGameNotFound()
    {
        // Arrange
        GameDto? game = null;

        string key = "key";

        _gameServiceMock.Setup(x => x.GetByKeyAsync(key)).ReturnsAsync(game);

        // Act
        var result = await _controller.GetByKey(key);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
        _gameServiceMock.Verify(x => x.GetByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task GetByIdShouldReturnOkResponseWhenGameFound()
    {
        // Arrange
        var gameDtoMock = _fixture.Create<GameDto>();
        var gameResponseMock = _fixture.Create<GameResponse>();

        Guid id = gameDtoMock.Id;

        _gameServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(gameDtoMock);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult<GameResponse>>();
        result.Result.Should().BeAssignableTo<OkObjectResult>();
        result.Result.As<OkObjectResult>().Value
            .Should()
            .NotBeNull()
            .And.BeOfType(gameResponseMock.GetType());
        _gameServiceMock.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdShouldReturnNotFoundResponseWhenGameNotFound()
    {
        // Arrange
        GameDto? game = null;

        Guid id = Guid.NewGuid();

        _gameServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(game);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
        _gameServiceMock.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task CreateShouldReturnOkResponseWhenValidRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateGameRequest>();

        var expectedDto = new CreateGameDto(
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.Game.Price,
            request.Game.UnitInStock,
            request.Game.Discount,
            request.Genres,
            request.Platforms,
            request.Publisher);

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateGameRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _gameServiceMock
            .Setup(x => x.CreateAsync(It.Is<CreateGameDto>(dto =>
                    dto.Name == expectedDto.Name &&
                    dto.Description == expectedDto.Description &&
                    dto.Key == expectedDto.Key &&
                    CompareNullableCollections(dto.GenresIds, request.Genres) &&
                    CompareNullableCollections(dto.PlatformsIds, request.Platforms))))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();

        result.Should().BeAssignableTo<OkResult>();
        _gameServiceMock.Verify(
            x => x.CreateAsync(It.Is<CreateGameDto>(dto =>
            dto.Name == expectedDto.Name &&
            dto.Description == expectedDto.Description &&
            dto.Key == expectedDto.Key &&
            CompareNullableCollections(dto.GenresIds, request.Genres) &&
            CompareNullableCollections(dto.PlatformsIds, request.Platforms))),
            Times.Once);
    }

    [Fact]
    public async Task UpdateShouldReturnNoContentResponseWhenValidRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        var expectedDto = new UpdateGameDto(
            request.Game.Id,
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.Game.Price,
            request.Game.UnitInStock,
            request.Game.Discount,
            request.Genres,
            request.Platforms,
            request.Publisher);

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateGameRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _gameServiceMock
            .Setup(x => x.UpdateAsync(It.Is<UpdateGameDto>(dto =>
                dto.Id == expectedDto.Id &&
                dto.Name == expectedDto.Name &&
                dto.Description == expectedDto.Description &&
                dto.Key == expectedDto.Key &&
                CompareNullableCollections(dto.GenresIds, request.Genres) &&
                CompareNullableCollections(dto.PlatformsIds, request.Platforms))))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();
        result.Should().BeAssignableTo<NoContentResult>();

        _gameServiceMock.Verify(
            x => x.UpdateAsync(It.Is<UpdateGameDto>(dto =>
                dto.Id == expectedDto.Id &&
                dto.Name == expectedDto.Name &&
                dto.Description == expectedDto.Description &&
                dto.Key == expectedDto.Key &&
                CompareNullableCollections(dto.GenresIds, request.Genres) &&
                CompareNullableCollections(dto.PlatformsIds, request.Platforms))),
            Times.Once);
    }

    [Fact]
    public async Task UpdateShouldReturnNotFoundWhenNotFoundExceptionThrown()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateGameRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _gameServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<UpdateGameDto>()))
            .ThrowsAsync(new NotFoundException("Game not found"));

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("Game not found");
    }

    [Fact]
    public async Task DeleteShouldReturnNoContentWhenValidKey()
    {
        // Arrange
        var key = _fixture.Create<string>();

        _gameServiceMock
            .Setup(x => x.DeleteByKeyAsync(key))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(key);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();
        result.Should().BeAssignableTo<NoContentResult>();

        _gameServiceMock.Verify(x => x.DeleteByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task DeleteShouldReturnNotFoundWhenNotFoundExceptionThrown()
    {
        // Arrange
        var key = _fixture.Create<string>();

        _gameServiceMock
            .Setup(x => x.DeleteByKeyAsync(key))
            .ThrowsAsync(new NotFoundException("Game not found"));

        // Act
        var result = await _controller.Delete(key);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be($"Game with key {key} not found");
    }

    [Fact]
    public async Task GetFileShouldReturnFileWhenGameFound()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var game = _fixture.Create<GameDto>();
        _gameServiceMock.Setup(x => x.GetByKeyAsync(key)).ReturnsAsync(game);

        // Act
        var result = await _controller.GetFile(key);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        var fileResult = result as FileContentResult;
        fileResult.FileContents.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(game)));
        fileResult.ContentType.Should().Be("text/plain");
        fileResult.FileDownloadName.Should().StartWith(game.Name);
    }

    [Fact]
    public async Task GetGenresByKeyReturnsOkWhenGenresAreFound()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var genres = _fixture.Create<ICollection<GenreShortDto>>();
        _genreServiceMock
            .Setup(x => x.GetAllByGameKeyAsync(key))
            .ReturnsAsync(genres);

        // Act
        var result = await _controller.GetGenresByKey(key);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(genres.Select(g => g.ToShortResponse()));
    }

    [Fact]
    public async Task GetGenresByKeyReturnsNotFoundWhenNotFoundExceptionThrown()
    {
        // Arrange
        var key = _fixture.Create<string>();
        _genreServiceMock
            .Setup(x => x.GetAllByGameKeyAsync(key))
            .ThrowsAsync(new NotFoundException("Genres not found"));

        // Act
        var result = await _controller.GetGenresByKey(key);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("Genres not found");
    }

    [Fact]
    public async Task GetPlatformsByKeyReturnsOkWhenPlatformsAreFound()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var platforms = _fixture.Create<ICollection<PlatformShortDto>>();
        _platformServiceMock
            .Setup(x => x.GetAllByGameKeyAsync(key))
            .ReturnsAsync(platforms);

        // Act
        var result = await _controller.GetPlatformsByKey(key);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(platforms.Select(p => p.ToShortResponse()));
    }

    private static bool CompareNullableCollections(ICollection<Guid>? list1, ICollection<Guid>? list2)
    {
#pragma warning disable SA1503, IDE0011
        if (list1 == null && list2 == null) return true;
        if (list1 == null || list2 == null) return false;
#pragma warning restore SA1503, IDE0011

        return list1.SequenceEqual(list2);
    }
}