using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.BLL.DTOs.Game;

public record GameDto(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    ICollection<GenreShortDto> Genres,
    ICollection<PlatformShortDto> Platforms);