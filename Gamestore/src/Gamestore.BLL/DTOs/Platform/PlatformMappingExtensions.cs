using Gamestore.BLL.DTOs.Game;
using PlatformEntity = Gamestore.DAL.Entities.Platform;

namespace Gamestore.BLL.DTOs.Platform;

public static class PlatformMappingExtensions
{
    public static PlatformDto ToDto(this PlatformEntity platform, ICollection<GameShortDto> games)
    {
        return new PlatformDto(
            platform.Id,
            platform.Type,
            games);
    }

    public static PlatformShortDto ToShortDto(this PlatformEntity platform)
    {
        return new PlatformShortDto(
            platform.Id,
            platform.Type);
    }

    public static PlatformEntity ToEntity(this CreatePlatformDto dto)
    {
        return new PlatformEntity
        {
            Type = dto.Type,
        };
    }

    public static void UpdateEntity(this UpdatePlatformDto dto, PlatformEntity platform)
    {
        platform.Type = dto.Type;
    }
}