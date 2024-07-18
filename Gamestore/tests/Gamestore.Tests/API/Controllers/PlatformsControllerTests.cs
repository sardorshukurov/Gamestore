using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Tests.API.Controllers;

public class PlatformsControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly Mock<IPlatformService> _platformService;

    private readonly Mock<CreatePlatformValidator> _createValidator;
    private readonly Mock<UpdatePlatformValidator> _updateValidator;

    private readonly PlatformsController _controller;

    public PlatformsControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _gameServiceMock = _fixture.Freeze<Mock<IGameService>>();
        _platformService = _fixture.Freeze<Mock<IPlatformService>>();

        _createValidator = _fixture.Freeze<Mock<CreatePlatformValidator>>();
        _updateValidator = _fixture.Freeze<Mock<UpdatePlatformValidator>>();

        _controller = new PlatformsController(
            _platformService.Object,
            _gameServiceMock.Object,
            _createValidator.Object,
            _updateValidator.Object);
    }

    [Fact]
    public async Task GetAllGamesReturnsOkWhenGamesAreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var games = _fixture.Create<ICollection<GameDto>>();
        _gameServiceMock.Setup(x => x.GetByPlatformAsync(id)).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGames(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(games.Select(g => g.AsResponse()));
    }

    [Fact]
    public async Task GetAllGamesReturns500WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        _gameServiceMock.Setup(x => x.GetByPlatformAsync(id)).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.GetAllGames(id);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }

    [Fact]
    public async Task CreateReturnsOkWhenCreationIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<CreatePlatformRequest>();

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreatePlatformRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _platformService.Setup(x => x.CreateAsync(request.AsDto())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<OkResult>();
        _platformService.Verify(x => x.CreateAsync(request.AsDto()), Times.Once);
    }

    [Fact]
    public async Task CreateReturns500WhenExceptionThrown()
    {
        // Arrange
        var request = _fixture.Create<CreatePlatformRequest>();
        _platformService.Setup(x => x.CreateAsync(request.AsDto())).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }

    [Fact]
    public async Task GetByIdReturnsOkWhenPlatformFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platform = _fixture.Create<PlatformShortDto>();
        _platformService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(platform);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(platform.AsShortResponse());
        okResult.Value.Should().BeOfType(platform.AsShortResponse().GetType());
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundWhenPlatformNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((PlatformShortDto)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be($"Platform with id {id} not found");
    }

    [Fact]
    public async Task GetByIdReturns500WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.GetByIdAsync(id)).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }

    [Fact]
    public async Task GetAllReturnsOkWhenPlatformsFound()
    {
        // Arrange
        var platforms = _fixture.Create<ICollection<PlatformShortDto>>();
        _platformService.Setup(x => x.GetAllAsync()).ReturnsAsync(platforms);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(platforms.Select(g => g.AsShortResponse()));
    }

    [Fact]
    public async Task GetAllReturns500WhenExceptionThrown()
    {
        // Arrange
        _platformService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }

    [Fact]
    public async Task UpdateReturnsNoContentWhenUpdateIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdatePlatformRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _platformService.Setup(x => x.UpdateAsync(request.AsDto())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateReturnsNotFoundWhenNotFoundExceptionThrown()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdatePlatformRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _platformService.Setup(x => x.UpdateAsync(request.AsDto())).ThrowsAsync(new NotFoundException("Platform not found"));

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("Platform not found");
    }

    [Fact]
    public async Task UpdateReturns500WhenExceptionThrown()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();
        _platformService.Setup(x => x.UpdateAsync(request.AsDto())).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }

    [Fact]
    public async Task DeleteReturnsNoContentWhenDeleteIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteReturnsNotFoundWhenNotFoundExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.DeleteAsync(id)).ThrowsAsync(new NotFoundException($"Platform with id {id} not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be($"Platform with id {id} not found");
    }

    [Fact]
    public async Task DeleteReturns500WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.DeleteAsync(id)).ThrowsAsync(new Exception("An internal server error has occured"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().Be("An internal server error has occured");
    }
}