using CommentEntity = Gamestore.Domain.Entities.Comments.Comment;

namespace Gamestore.BLL.DTOs.Comment;

public static class CommentMappingExtensions
{
    public static CommentEntity ToEntity(this CreateCommentRequest request, string bodyMessage, Guid gameId, string userName)
    {
        return new CommentEntity
        {
            Name = userName,
            Body = bodyMessage,
            ParentCommentId = request.ParentId,
            GameId = gameId,
        };
    }
}