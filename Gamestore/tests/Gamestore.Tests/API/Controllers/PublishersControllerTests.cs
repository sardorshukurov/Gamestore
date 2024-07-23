using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Publisher;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Tests.API.Controllers;

public class PublishersControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPublisherService> _publisherServiceMock;
    private readonly Mock<IGameService> _gameServiceMock;

    private readonly Mock<CreatePublisherValidator> _createValidatorMock;
    private readonly Mock<UpdatePublisherValidator> _updateValidatorMock;

    private readonly PublishersController _controller;

    public PublishersControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _publisherServiceMock = _fixture.Freeze<Mock<IPublisherService>>();
        _gameServiceMock = _fixture.Freeze<Mock<IGameService>>();

        _createValidatorMock = _fixture.Freeze<Mock<CreatePublisherValidator>>();
        _updateValidatorMock = _fixture.Freeze<Mock<UpdatePublisherValidator>>();

        _controller = new PublishersController(
            _publisherServiceMock.Object,
            _gameServiceMock.Object);
    }

    [Fact]
    public async Task CreateValidRequestCallsServiceAndReturnsOk()
    {
        // Arrange
        var request = _fixture.Create<CreatePublisherRequest>();
        var validationResult = new FluentValidation.Results.ValidationResult();
        _createValidatorMock.Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<CreatePublisherRequest>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.Create(request);

        // Assert
        Assert.IsType<OkResult>(result);
        _publisherServiceMock.Verify(x => x.CreateAsync(It.IsAny<CreatePublisherDto>()), Times.Once);
    }

    [Fact]
    public async Task GetByCompanyNamePublisherExistsReturnsOk()
    {
        // Arrange
        var publisherDto = _fixture.Create<PublisherDto>();
        _publisherServiceMock.Setup(x => x.GetByCompanyNameAsync(It.IsAny<string>()))
            .ReturnsAsync(publisherDto);

        // Act
        var result = await _controller.GetByCompanyName("companyName");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PublisherResponse>(okResult.Value);
        Assert.Equal(publisherDto.Id, returnValue.Id);
    }

    [Fact]
    public async Task GetByCompanyNamePublisherDoesNotExistReturnsNotFound()
    {
        // Arrange
        _publisherServiceMock.Setup(x => x.GetByCompanyNameAsync(It.IsAny<string>()))
            .ReturnsAsync((PublisherDto)null);

        // Act
        var result = await _controller.GetByCompanyName("companyName");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReturnsListOfPublishers()
    {
        // Arrange
        var publishers = _fixture.Create<List<PublisherDto>>();
        _publisherServiceMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(publishers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<PublisherResponse>>(okResult.Value);
        Assert.Equal(publishers.Count, returnValue.Count());
        _publisherServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateValidRequestCallsServiceAndUpdate()
    {
        // Arrange
        var request = _fixture.Create<UpdatePublisherRequest>();
        var validationResult = new FluentValidation.Results.ValidationResult();
        _updateValidatorMock.Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<UpdatePublisherRequest>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.Update(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _publisherServiceMock.Verify(x => x.UpdateAsync(It.IsAny<UpdatePublisherDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateNotFoundExceptionReturnsNotFound()
    {
        // Arrange
        var request = _fixture.Create<UpdatePublisherRequest>();
        var validationResult = new FluentValidation.Results.ValidationResult();
        _updateValidatorMock.Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<UpdatePublisherRequest>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _publisherServiceMock.Setup(x => x.UpdateAsync(It.IsAny<UpdatePublisherDto>()))
            .ThrowsAsync(new NotFoundException("Not found"));

        // Act
        var result = await _controller.Update(request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteValidIdCallsServiceAndDeletesPublisher()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        _publisherServiceMock.Setup(x => x.DeleteAsync(id));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _publisherServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteNotExistingIdReturnsNotFoundResult()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        _publisherServiceMock.Setup(x => x.DeleteAsync(id))
            .Throws(new NotFoundException($"Publisher with id {id} not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _publisherServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetGamesByCompanyNamePublisherExistsReturnsOk()
    {
        // Arrange
        var games = _fixture.Create<List<GameDto>>();
        _gameServiceMock.Setup(x => x.GetByPublisherAsync(It.IsAny<string>()))
            .ReturnsAsync(games);

        // Act
        var result = await _controller.GetGamesByCompanyName("companyName");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<GameResponse>>(okResult.Value);
        Assert.Equal(games.Count, returnValue.Count);
    }

    [Fact]
    public async Task GetGamesByCompanyNamePublisherDoesNotExistReturnsNotFound()
    {
        // Arrange
        _gameServiceMock.Setup(x => x.GetByPublisherAsync(It.IsAny<string>()))
            .Throws(new NotFoundException("Not found"));

        // Act
        var result = await _controller.GetGamesByCompanyName("companyName");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}