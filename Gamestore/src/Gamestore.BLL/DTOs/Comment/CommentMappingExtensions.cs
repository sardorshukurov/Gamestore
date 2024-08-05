using CommentEntity = Gamestore.Domain.Entities.Comment;

namespace Gamestore.BLL.DTOs.Comment;

public static class CommentMappingExtensions
{
    public static CommentResponse ToResponse(this CommentEntity entity)
    {
        return new CommentResponse(
            entity.Id,
            entity.Name,
            entity.Body,
            entity.Replies
                .Select(r => r.ToResponse())
                .ToList());
    }

    public static CommentEntity ToEntity(this CreateCommentRequest request, string bodyMessage, Guid gameId)
    {
        return new CommentEntity
        {
            Name = request.Name,
            Body = bodyMessage,
            ParentCommentId = request.ParentId,
            GameId = gameId,
        };
    }
}