namespace Gamestore.BLL.DTOs.Platform;

public record CreatePlatformDto(
    string Type,
    ICollection<Guid>? GameIds);