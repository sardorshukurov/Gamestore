using Gamestore.Domain.Entities.Comments;

namespace Gamestore.BLL.DTOs.Comment.Ban;

public record BanUserRequest(
    Guid UserId,
    BanDuration Duration);