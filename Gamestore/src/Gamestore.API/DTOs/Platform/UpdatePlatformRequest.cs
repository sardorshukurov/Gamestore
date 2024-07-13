namespace Gamestore.API.DTOs.Platform;

public record UpdatePlatformRequest(
    Guid Id,
    string Type);