namespace Gamestore.API.DTOs.Game;

public record GameResponse(
    Guid Id,
    string Name,
    string Key,
    string? Description);