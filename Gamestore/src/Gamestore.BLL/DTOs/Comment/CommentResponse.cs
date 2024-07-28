namespace Gamestore.BLL.DTOs.Comment;

public record CommentResponse(
    Guid Id,
    string Name,
    string Body,
    ICollection<CommentResponse> ChildComments);