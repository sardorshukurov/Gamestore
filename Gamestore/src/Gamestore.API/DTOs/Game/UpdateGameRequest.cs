namespace Gamestore.API.DTOs.Game;

public record UpdateGameRequest(
    UpdateGame Game,
    ICollection<Guid>? GenresIds,
    ICollection<Guid>? PlatformsIds);

public record UpdateGame(
    Guid Id,
    string Name,
    string? Key,
    string? Description);