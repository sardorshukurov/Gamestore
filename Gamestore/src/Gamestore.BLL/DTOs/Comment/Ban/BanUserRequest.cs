namespace Gamestore.BLL.DTOs.Comment.Ban;

public record BanUserRequest(
    string User,
    string Duration);