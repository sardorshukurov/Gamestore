using Gamestore.BLL.DTOs.Game;

namespace Gamestore.BLL.DTOs.Platform;

public record PlatformResponse(
    Guid Id,
    string Type,
    ICollection<GameShortResponse> Games);