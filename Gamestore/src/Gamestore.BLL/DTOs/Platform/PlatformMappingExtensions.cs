using Gamestore.BLL.DTOs.Game;
using PlatformEntity = Gamestore.Domain.Entities.Platform;

namespace Gamestore.BLL.DTOs.Platform;

public static class PlatformMappingExtensions
{
    public static PlatformResponse ToResponse(this PlatformEntity entity, ICollection<GameShortResponse> games)
    {
        return new PlatformResponse(
            entity.Id,
            entity.Type,
            games);
    }

    public static PlatformShortResponse ToShortResponse(this PlatformEntity dto)
    {
        return new PlatformShortResponse(
            dto.Id,
            dto.Type);
    }

    public static PlatformEntity ToEntity(this CreatePlatformRequest request)
    {
        return new PlatformEntity
        {
            Type = request.Type,
        };
    }

    public static void UpdateEntity(this UpdatePlatformRequest request, PlatformEntity entity)
    {
        entity.Id = request.Id;
        entity.Type = request.Type;
    }
}