using Gamestore.BLL.DTOs.Game;

namespace Gamestore.BLL.DTOs.Platform;

public record PlatformDto(
    Guid Id,
    string Type,
    ICollection<GameShortDto> Games);