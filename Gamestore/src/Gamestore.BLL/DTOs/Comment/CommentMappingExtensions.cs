using CommentEntity = Gamestore.Domain.Entities.Comment;

namespace Gamestore.BLL.DTOs.Comment;

public static class CommentMappingExtensions
{
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