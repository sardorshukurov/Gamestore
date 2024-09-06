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
        var createCommentRequest = new CreateCommentRequest("Body", null, null);
        var gameKey = _fixture.Create<string>();

        _commentRepositoryMock.Setup(
                x =>
                x.CreateAsync(It.IsAny<Comment>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddCommentAsync(gameKey, createCommentRequest, string.Empty, Guid.NewGuid());

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
        var bannedUserId = _fixture.Create<Guid>();
        var createCommentRequest = _fixture.Build<CreateCommentRequest>()
            .Create();
        var gameKey = _fixture.Create<string>();

        _banRepositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<Ban, bool>>>()))
            .ReturnsAsync(new Ban
            {
                UserId = bannedUserId,
                EndDate = DateTime.Today.AddDays(1),
            });

        // Act & Assert
        await Assert.ThrowsAsync<UserIsBannedException>(() => _service.AddCommentAsync(gameKey, createCommentRequest, string.Empty, bannedUserId));
    }

    [Fact]
    public async Task AddCommentAsyncShouldProceedIfUserIsNotBanned()
    {
        // Arrange
        var createCommentRequest = new CreateCommentRequest("Body", null, null);
        var gameKey = _fixture.Create<string>();

        _banRepositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<Ban, bool>>>()))
            .ReturnsAsync((Ban)null); // No active bans found

        // Act
        await _service.AddCommentAsync(gameKey, createCommentRequest, string.Empty, Guid.NewGuid());

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
}