namespace Gamestore.BLL.DTOs.User;

public record AuthRequest(
    string Email,
    string Password);