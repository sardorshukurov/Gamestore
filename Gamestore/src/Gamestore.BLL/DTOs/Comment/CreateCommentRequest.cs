namespace Gamestore.BLL.DTOs.Comment;

public record CreateCommentRequest(
    string Name,
    string Body,
    Guid? ParentId,
    CommentAction? Action);