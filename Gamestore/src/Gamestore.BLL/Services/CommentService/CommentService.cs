using Gamestore.BLL.DTOs.Comment;
using Gamestore.BLL.DTOs.Comment.Ban;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Services.CommentService;

public class CommentService(
    IRepository<Comment> commentRepository,
    IRepository<Game> gameRepository) : ICommentService
{
    public Task AddCommentAsync(string gameKey, CreateCommentRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<CommentResponse>> GetAllCommentsByGameAsync(string gameKey)
    {
        var game = await gameRepository.GetOneAsync(g => g.Key == gameKey)
                   ?? throw new GameNotFoundException(gameKey);

        var allComments = (await GetAllCommentsByGameIdAsync(game.Id))
            .ToList();

        var topLevelComments = allComments
            .Where(c => c.ParentCommentId == null)
            .ToList();

        var commentResponses = topLevelComments
            .Select(c => BuildCommentResponse(c, allComments))
            .ToList();

        return commentResponses;
    }

    public Task DeleteCommentByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public ICollection<string> GetBanDurations()
    {
        return BanDurationResponse.BanDurations;
    }

    public Task BanUserAsync(BanUserRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<ICollection<Comment>> GetAllCommentsByGameIdAsync(Guid gameId)
    {
        var comments = (await commentRepository
            .GetAllByFilterAsync(c => c.GameId == gameId))
            .ToList();

        return comments;
    }

    private static CommentResponse BuildCommentResponse(Comment comment, List<Comment> allComments)
    {
        var childComments = allComments
            .Where(c => c.ParentCommentId == comment.Id)
            .Select(c => BuildCommentResponse(c, allComments))
            .ToList();

        return new CommentResponse(
            comment.Id,
            comment.Name,
            comment.Body,
            childComments);
    }
}