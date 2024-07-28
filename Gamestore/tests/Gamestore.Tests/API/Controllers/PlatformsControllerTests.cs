using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Tests.API.Controllers;

public class PlatformsControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPlatformService> _platformService;

    private readonly Mock<CreatePlatformValidator> _createValidator;
    private readonly Mock<UpdatePlatformValidator> _updateValidator;

    private readonly PlatformsController _controller;

    public PlatformsControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _platformService = _fixture.Freeze<Mock<IPlatformService>>();

        _createValidator = _fixture.Freeze<Mock<CreatePlatformValidator>>();
        _updateValidator = _fixture.Freeze<Mock<UpdatePlatformValidator>>();

        _controller = new PlatformsController(
            _platformService.Object);
    }

    [Fact]
    public async Task CreateReturnsOkWhenCreationIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<CreatePlatformRequest>();

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreatePlatformRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _platformService.Setup(x => x.CreateAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<OkResult>();
        _platformService.Verify(x => x.CreateAsync(request), Times.Once);
    }

    [Fact]
    public async Task GetByIdReturnsOkWhenPlatformFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platform = _fixture.Create<PlatformShortResponse>();
        _platformService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(platform);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(platform);
        okResult.Value.Should().BeOfType(platform.GetType());
    }

    [Fact]
    public async Task GetByIdReturnsNotFoundWhenPlatformNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _platformService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((PlatformShortResponse)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be($"Platform with id {id} not found");
    }

    [Fact]
    public async Task GetAllReturnsOkWhenPlatformsFound()
    {
        // Arrange
        var platforms = _fixture.Create<ICollection<PlatformShortResponse>>();
        _platformService.Setup(x => x.GetAllAsync()).ReturnsAsync(platforms);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(platforms);
    }

    [Fact]
    public async Task UpdateReturnsNoContentWhenUpdateIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<UpdatePlatformRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdatePlatformRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _platformService.Setup(x => x.UpdateAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
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
}