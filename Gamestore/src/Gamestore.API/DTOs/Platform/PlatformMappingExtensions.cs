using Gamestore.API.DTOs.Game;
using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.API.DTOs.Platform;

public static class PlatformMappingExtensions
{
    public static PlatformResponse AsResponse(this PlatformDto dto, ICollection<GameShortResponse> games)
    {
        return new PlatformResponse(
            dto.Id,
            dto.Type,
            games);
    }

    public static PlatformShortResponse AsShortResponse(this PlatformDto dto)
    {
        return new PlatformShortResponse(
            dto.Id,
            dto.Type);
    }

    public static CreatePlatformDto AsDto(this CreatePlatformRequest request)
    {
        return new CreatePlatformDto(
            request.Type,
            null);
    }

    public static UpdatePlatformDto AsDto(this UpdatePlatformRequest request)
    {
        return new UpdatePlatformDto(
            request.Id,
            request.Type,
            null);
    }
}