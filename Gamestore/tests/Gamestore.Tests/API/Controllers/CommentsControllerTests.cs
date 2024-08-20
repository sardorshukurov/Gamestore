using FluentValidation;
using Gamestore.API.Controllers;
using Gamestore.BLL.DTOs.Comment;
using Gamestore.BLL.DTOs.Comment.Ban;
using Gamestore.BLL.Services.CommentService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.Tests.API.Controllers;

public class CommentsControllerTests
{
    private readonly IFixture _fixture;

    private readonly Mock<ICommentService> _commentServiceMock;

    private readonly Mock<CreateCommentValidator> _createValidator;

    private readonly CommentsController _controller;

    public CommentsControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));

        _commentServiceMock = _fixture.Freeze<Mock<ICommentService>>();

        _createValidator = _fixture.Freeze<Mock<CreateCommentValidator>>();

        _controller = new CommentsController(_commentServiceMock.Object);
    }

    [Fact]
    public async Task GetAllCommentsByGameKeyReturnsComments()
    {
        // Arrange
        var comments = _fixture.CreateMany<CommentResponse>(5).ToList();
        var gameKey = _fixture.Create<string>();

        _commentServiceMock.Setup(x => x.GetAllCommentsByGameAsync(gameKey))
            .ReturnsAsync(comments);

        // Act
        var result = await _controller.GetAllCommentsByGameKey(gameKey);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult<IEnumerable<CommentResponse>>>();

        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().NotBeNull();

        var commentResponse = okResult.Value as IEnumerable<CommentResponse>;
        commentResponse.Should().NotBeNull();
        commentResponse.Should().HaveCount(comments.Count);

        _commentServiceMock.Verify(x => x.GetAllCommentsByGameAsync(gameKey), Times.Once);
    }

    [Fact]
    public async Task CreateShouldReturnOkResponseWhenValidRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateCommentRequest>();
        var gameKey = _fixture.Create<string>();

        _createValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateCommentRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _commentServiceMock
            .Setup(x => x.AddCommentAsync(
                It.IsAny<string>(),
                It.IsAny<CreateCommentRequest>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddComment(gameKey, request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();

        result.Should().BeAssignableTo<OkResult>();
        _commentServiceMock.Verify(
            x =>
                x.AddCommentAsync(It.IsAny<string>(), It.IsAny<CreateCommentRequest>()),
            Times.Once);
    }

    [Fact]
    public async Task AddCommentServiceThrowsExceptionResultsIn500()
    {
        // Arrange
        var gameKey = _fixture.Create<string>();
        var request = _fixture.Create<CreateCommentRequest>();

        _commentServiceMock.Setup(x => x.AddCommentAsync(gameKey, request))
            .ThrowsAsync(new Exception("Internal service error"));

        // Act
        Func<Task> action = async () => await _controller.AddComment(gameKey, request);

        // Assert
        await Assert.ThrowsAsync<Exception>(action);
    }

    [Fact]
    public async Task DeleteCommentValidIdReturnsNoContent()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();
        _commentServiceMock.Setup(x => x.DeleteCommentByIdAsync(commentId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteComment(commentId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void GetBanDurationsReturnsCorrectDurations()
    {
        // Arrange
        var durations = new List<string> { "1 Day", "1 Week", "1 Month" };
        _commentServiceMock.Setup(x => x.GetBanDurations()).Returns(durations);

        // Act
        var result = _controller.GetBanDurations();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var actualDurations = okResult.Value as IEnumerable<string>;
        Assert.Equal(durations.Count, actualDurations!.Count());
    }

    [Fact]
    public async Task BanUserValidRequestReturnsNoContent()
    {
        // Arrange
        var request = _fixture.Create<BanUserRequest>();
        _commentServiceMock.Setup(x => x.BanUserAsync(request)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.BanUser(request);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}