using System.Linq.Expressions;
using Gamestore.BLL.DTOs.Comment;
using Gamestore.BLL.Services.CommentService;
using Gamestore.Common.Exceptions.BadRequest;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Comments;
using Gamestore.Domain.Entities.Games;

namespace Gamestore.Tests.BLL.Services;

public class CommentServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRepository<Comment>> _commentRepositoryMock;
    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<Ban>> _banRepositoryMock;

    private readonly CommentService _service;

    public CommentServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _commentRepositoryMock = _fixture.Freeze<Mock<IRepository<Comment>>>();
        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _banRepositoryMock = _fixture.Freeze<Mock<IRepository<Ban>>>();

        _service = new CommentService(
            _commentRepositoryMock.Object,
            _gameRepositoryMock.Object,
            _banRepositoryMock.Object);
    }

    [Fact]
    public async Task AddCommentAsyncShouldAddComment()
    {
        // Assert
        var createCommentRequest = new CreateCommentRequest("Name", "Body", null, null);
        var gameKey = _fixture.Create<string>();

        _commentRepositoryMock.Setup(
                x =>
                x.CreateAsync(It.IsAny<Comment>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddCommentAsync(gameKey, createCommentRequest);

        // Assert
        _commentRepositoryMock.Verify(
            x => x.CreateAsync(
            It.IsAny<Comment>()),
            Times.Once);
        _commentRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AddCommentAsyncShouldThrowUserIsBannedExceptionIfUserIsBanned()
    {
        // Arrange
        var bannedUserName = _fixture.Create<string>();
        var createCommentRequest = _fixture.Build<CreateCommentRequest>()
            .With(c => c.Name, bannedUserName)
            .Create();
        var gameKey = _fixture.Create<string>();

        _banRepositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<Ban, bool>>>()))
            .ReturnsAsync(new Ban
            {
                UserName = bannedUserName,
                EndDate = DateTime.Today.AddDays(1),
            });

        // Act & Assert
        await Assert.ThrowsAsync<UserIsBannedException>(() => _service.AddCommentAsync(gameKey, createCommentRequest));
    }

    [Fact]
    public async Task AddCommentAsyncShouldProceedIfUserIsNotBanned()
    {
        // Arrange
        var createCommentRequest = new CreateCommentRequest("Name", "Body", null, null);
        var gameKey = _fixture.Create<string>();

        _banRepositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<Ban, bool>>>()))
            .ReturnsAsync((Ban)null); // No active bans found

        // Act
        await _service.AddCommentAsync(gameKey, createCommentRequest);

        // Assert
        _commentRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Comment>()), Times.Once());
    }

    [Fact]
    public async Task DeleteCommentByIdAsyncMarksParentAndChildCommentsAsDeleted()
    {
        // Arrange
        var parentId = _fixture.Create<Guid>();

        _commentRepositoryMock.Setup(x => x.DeleteByIdAsync(parentId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteCommentByIdAsync(parentId);

        // Assert
        _commentRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task DeleteCommentByIdAsyncShouldThrowIfCommentNotFound()
    {
        // Arrange
        var commentId = _fixture.Create<Guid>();

        _commentRepositoryMock.Setup(x => x.GetByIdAsync(commentId)).ReturnsAsync((Comment)null);

        // Act & Assert
        await Assert.ThrowsAsync<CommentNotFoundException>(() => _service.DeleteCommentByIdAsync(commentId));
    }

    [Fact]
    public async Task GetAllCommentsByGameAsyncReturnsTopLevelComments()
    {
        // Arrange
        var gameKey = _fixture.Create<string>();
        var gameId = _fixture.Create<Guid>();

        // Create a mix of top-level comments (ParentCommentId = null) and child comments.
        var topLevelComments = _fixture.Build<Comment>()
            .With(c => c.GameId, gameId)
            .Without(c => c.ParentCommentId)
            .CreateMany(3).ToList();
        var childComments = _fixture.Build<Comment>()
            .With(c => c.GameId, gameId)
            .With(c => c.ParentCommentId, () => _fixture.Create<Guid>()) // Non-null ParentCommentId
            .CreateMany(2).ToList();

        var comments = topLevelComments.Concat(childComments).ToList();

        _gameRepositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<Game, bool>>>()))
            .ReturnsAsync(new Game { Id = gameId, Key = gameKey });
        _commentRepositoryMock.Setup(x => x.GetAllByFilterAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
            .ReturnsAsync(comments);

        // Act
        var result = await _service.GetAllCommentsByGameAsync(gameKey);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(topLevelComments.Count, result.Count); // Ensure only top-level comments are returned.
    }
}