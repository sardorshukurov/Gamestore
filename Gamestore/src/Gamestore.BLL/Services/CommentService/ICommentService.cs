using Gamestore.BLL.DTOs.Comment;
using Gamestore.BLL.DTOs.Comment.Ban;

namespace Gamestore.BLL.Services.CommentService;

public interface ICommentService
{
    Task AddCommentAsync(string gameKey, CreateCommentRequest request);

    Task<ICollection<CommentResponse>> GetAllCommentsByGameAsync(string gameKey);

    Task DeleteCommentByIdAsync(Guid id);

    ICollection<string> GetBanDurations();

    Task BanUserAsync(BanUserRequest request);
}