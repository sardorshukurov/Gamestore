namespace Gamestore.BLL.DTOs.Game;

// TODO: make key nullable and generate it if it is null
public record CreateGameDto(
    string Name,
    string Key,
    string? Description,
    ICollection<Guid>? GenresIds,
    ICollection<Guid>? PlatformsIds);