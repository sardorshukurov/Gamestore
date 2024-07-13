using Gamestore.API.DTOs.Game;

namespace Gamestore.API.DTOs.Platform;

public record PlatformResponse(
    Guid Id,
    string Type,
    ICollection<GameShortResponse> Games);