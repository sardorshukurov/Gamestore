using Gamestore.API.DTOs.Game;
using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.API.DTOs.Platform;

public static class PlatformMappingExtensions
{
    public static PlatformResponse ToResponse(this PlatformDto dto, ICollection<GameShortResponse> games)
    {
        return new PlatformResponse(
            dto.Id,
            dto.Type,
            games);
    }

    public static PlatformShortResponse ToShortResponse(this PlatformDto dto)
    {
        return new PlatformShortResponse(
            dto.Id,
            dto.Type);
    }

    public static PlatformShortResponse ToShortResponse(this PlatformShortDto dto)
    {
        return new PlatformShortResponse(
            dto.Id,
            dto.Type);
    }

    public static CreatePlatformDto ToDto(this CreatePlatformRequest request)
    {
        return new CreatePlatformDto(
            request.Platform.Type,
            null);
    }

    public static UpdatePlatformDto ToDto(this UpdatePlatformRequest request)
    {
        return new UpdatePlatformDto(
            request.Platform.Id,
            request.Platform.Type,
            null);
    }
}