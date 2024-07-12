namespace Gamestore.BLL.DTOs.Game;

public record UpdateGameDto(
    string Name,
    string Key,
    string? Description,
    ICollection<Guid>? GenresIds,
    ICollection<Guid>? PlatformsIds);