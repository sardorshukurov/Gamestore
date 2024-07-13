namespace Gamestore.BLL.DTOs.Game;

public record CreateGameDto(
    string Name,
    string? Key,
    string? Description,
    ICollection<Guid>? GenresIds,
    ICollection<Guid>? PlatformsIds);