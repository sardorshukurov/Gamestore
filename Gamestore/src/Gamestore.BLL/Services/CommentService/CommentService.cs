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
    // TODO: add ban check
    public async Task AddCommentAsync(string gameKey, CreateCommentRequest request)
    {
        var game = await GetGameByKeyOrThrow(gameKey);
        string message = request.Body;

        if (request.ParentId is not null)
        {
            await CheckIfCommentsGamesMatch((Guid)request.ParentId, gameKey);

            var authorComment = await commentRepository.GetOneAsync(c => c.Id == request.ParentId)
                                ?? throw new CommentNotFoundException((Guid)request.ParentId);

            message = request.Action switch
            {
                CommentAction.Reply => $"{authorComment.Name}, {request.Body}",
                CommentAction.Quote => $"{authorComment.Body}, {request.Body}",
                _ => message,
            };
        }

        var commentToAdd = request.ToEntity(message, game.Id);

        await commentRepository.CreateAsync(commentToAdd);
        await commentRepository.SaveChangesAsync();
    }

    public async Task<ICollection<CommentResponse>> GetAllCommentsByGameAsync(string gameKey)
    {
        var game = await GetGameByKeyOrThrow(gameKey);

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

    public async Task DeleteCommentByIdAsync(Guid id)
    {
        var commentToDelete = await commentRepository.GetByIdAsync(id) ?? throw new CommentNotFoundException(id);

        commentToDelete.Body = "A comment/quote was deleted";

        await DeleteChildComments(id);

        await commentRepository.SaveChangesAsync();
    }

    public ICollection<string> GetBanDurations()
    {
        return BanDurationResponse.BanDurations.Values;
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

    private async Task<Game> GetGameByKeyOrThrow(string gameKey)
    {
        return await gameRepository.GetOneAsync(g => g.Key == gameKey)
            ?? throw new GameNotFoundException(gameKey);
    }

    private async Task<Comment> GetCommentByIdOrThrow(Guid id)
    {
        return await commentRepository.GetByIdAsync(id)
               ?? throw new CommentNotFoundException(id);
    }

    private async Task CheckIfCommentsGamesMatch(Guid parentCommentId, string gameKey)
    {
        var game = await GetGameByKeyOrThrow(gameKey);
        var parentComment = await GetCommentByIdOrThrow(parentCommentId);

        if (game.Id != parentComment.GameId)
        {
            throw new CommentGameMismatchException();
        }
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

    private async Task DeleteChildComments(Guid parentId)
    {
        var childComments = await commentRepository.GetAllByFilterAsync(c => c.ParentCommentId == parentId);

        foreach (var childComment in childComments)
        {
            childComment.Body = "A comment/quote was deleted";
            await commentRepository.UpdateAsync(childComment.Id, childComment);

            await DeleteChildComments(childComment.Id);
        }
    }
}