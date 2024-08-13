using System.Text;
using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.OrderService;
using Gamestore.DAL.Filtration.Games;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.Tests.API.Controllers;

public class GamesControllerTests
{
    private readonly IFixture _fixture;

    private readonly Mock<IGameService> _gameServiceMock;
    private readonly Mock<IOrderService> _orderServiceMock;

    private readonly Mock<CreateGameValidator> _createValidator;
    private readonly Mock<UpdateGameValidator> _updateValidator;

    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _gameServiceMock = _fixture.Freeze<Mock<IGameService>>();
        _orderServiceMock = _fixture.Freeze<Mock<IOrderService>>();

        _createValidator = _fixture.Freeze<Mock<CreateGameValidator>>();
        _updateValidator = _fixture.Freeze<Mock<UpdateGameValidator>>();

        _controller = new GamesController(
            _gameServiceMock.Object,
            _orderServiceMock.Object);
    }

    [Fact]
    public async Task GetAllShouldReturnOkResponse()
    {
        // Arrange
        var gamesResponse = _fixture.Create<GamesResponse>();

        _gameServiceMock.Setup(x => x.GetAllAsync(It.IsAny<SearchCriteria>())).ReturnsAsync(gamesResponse);

        // Act
        var result = await _controller.GetAll(It.IsAny<SearchCriteria>());

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<OkObjectResult>();

        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().NotBeNull();

        var response = okResult.Value as GamesResponse;
        response.Should().NotBeNull();
        response.Games.Should().HaveCount(gamesResponse.Games.Count);

        _gameServiceMock.Verify(x => x.GetAllAsync(It.IsAny<SearchCriteria>()), Times.Once);
    }

    [Fact]
    public async Task GetAllGamesByPlatformReturnsOkWhenGamesAreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var games = _fixture.Create<ICollection<GameResponse>>();
        _gameServiceMock.Setup(x => x.GetByPlatformAsync(id)).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGamesByPlatform(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(games);
    }

    [Fact]
    public async Task GetAllGamesByGenreReturnsOkWhenGamesAreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var games = _fixture.Create<ICollection<GameResponse>>();
        _gameServiceMock.Setup(x => x.GetByGenreAsync(id)).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGamesByGenre(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(games);
    }

    [Fact]
    public async Task GetByKeyShouldReturnOkResponseWhenGameFound()
    {
        // Arrange
        var gameDtoMock = _fixture.Create<GameResponse>();
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
        GameResponse? game = null;

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
        var gameDtoMock = _fixture.Create<GameResponse>();
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
        GameResponse? game = null;

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

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateGameRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _gameServiceMock
            .Setup(x => x.CreateAsync(It.IsAny<CreateGameRequest>()))
                .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();

        result.Should().BeAssignableTo<OkResult>();
        _gameServiceMock.Verify(
            x =>
                x.CreateAsync(It.IsAny<CreateGameRequest>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateShouldReturnNoContentResponseWhenValidRequest()
    {
        // Arrange
        var request = _fixture.Create<UpdateGameRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateGameRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _gameServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<UpdateGameRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();
        result.Should().BeAssignableTo<NoContentResult>();

        _gameServiceMock.Verify(
            x => x.UpdateAsync(It.IsAny<UpdateGameRequest>()),
            Times.Once);
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
    public async Task GetFileShouldReturnFileWhenGameFound()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var game = _fixture.Create<GameResponse>();
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
    public async Task GetGamesByCompanyNamePublisherExistsReturnsOk()
    {
        // Arrange
        var games = _fixture.Create<List<GameResponse>>();
        _gameServiceMock.Setup(x => x.GetByPublisherAsync(It.IsAny<string>()))
            .ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGamesByPublisher("companyName");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<GameResponse>>(okResult.Value);
        Assert.Equal(games.Count, returnValue.Count);
    }
}