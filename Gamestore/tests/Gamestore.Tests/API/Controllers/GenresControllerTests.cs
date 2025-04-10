using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.Services.GenreService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Tests.API.Controllers;

public class GenresControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IGenreService> _genreServiceMock;
    private readonly GenresController _controller;

    private readonly Mock<CreateGenreValidator> _createValidator;
    private readonly Mock<UpdateGenreValidator> _updateValidator;

    public GenresControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _genreServiceMock = _fixture.Freeze<Mock<IGenreService>>();

        _createValidator = _fixture.Freeze<Mock<CreateGenreValidator>>();
        _updateValidator = _fixture.Freeze<Mock<UpdateGenreValidator>>();

        _controller = new GenresController(
            _genreServiceMock.Object);
    }

    [Fact]
    public async Task CreateReturnsOkWhenCreationIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<CreateGenreRequest>();

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateGenreRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _genreServiceMock.Setup(x => x.CreateAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Should().BeOfType<OkResult>();
        _genreServiceMock.Verify(x => x.CreateAsync(request), Times.Once);
    }

    [Fact]
    public async Task GetGenreReturnsOkWhenGenreFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var genre = _fixture.Create<GenreShortResponse>();
        _genreServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(genre);

        // Act
        var result = await _controller.GetGenre(id);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(genre);
        okResult.Value.Should().BeOfType(genre.GetType());
    }

    [Fact]
    public async Task GetGenreReturnsNotFoundWhenGenreNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _genreServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((GenreShortResponse)null);

        // Act
        var result = await _controller.GetGenre(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be($"Genre with id {id} not found");
    }

    [Fact]
    public async Task GetAllReturnsOkWhenGenresFound()
    {
        // Arrange
        var genres = _fixture.Create<ICollection<GenreShortResponse>>();
        _genreServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(genres);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(genres);
    }

    [Fact]
    public async Task GetSubGenresReturnsOkWhenGenresFound()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var genres = _fixture.Create<ICollection<GenreShortResponse>>();
        _genreServiceMock.Setup(x => x.GetSubGenresAsync(parentId)).ReturnsAsync(genres);

        // Act
        var result = await _controller.GetSubGenres(parentId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(genres);
    }

    [Fact]
    public async Task UpdateReturnsNoContentWhenUpdateIsSuccessful()
    {
        // Arrange
        var request = _fixture.Create<UpdateGenreRequest>();

        _updateValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateGenreRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _genreServiceMock.Setup(x => x.UpdateAsync(request)).Returns(Task.CompletedTask);

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
        _genreServiceMock.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetGenresByKeyReturnsOkWhenGenresAreFound()
    {
        // Arrange
        var key = _fixture.Create<string>();
        var genres = _fixture.Create<ICollection<GenreShortResponse>>();
        _genreServiceMock
            .Setup(x => x.GetAllByGameKeyAsync(key))
            .ReturnsAsync(genres);

        // Act
        var result = await _controller.GetGenresByKey(key);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(genres);
    }
}