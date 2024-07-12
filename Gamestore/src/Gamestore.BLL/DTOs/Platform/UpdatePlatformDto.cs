namespace Gamestore.BLL.DTOs.Platform;

public record UpdatePlatformDto(
    string Type,
    ICollection<Guid>? GameIds);