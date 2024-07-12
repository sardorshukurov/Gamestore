namespace Gamestore.BLL.DTOs.Platform;

public record UpdatePlatformDto(
    Guid Id,
    string Type,
    ICollection<Guid>? GameIds);