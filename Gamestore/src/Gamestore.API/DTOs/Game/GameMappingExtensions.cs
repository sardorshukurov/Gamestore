using Gamestore.BLL.DTOs.Game;

namespace Gamestore.API.DTOs.Game;

public static class GameMappingExtensions
{
    public static GameResponse AsResponse(this GameDto dto)
    {
        return new GameResponse(
            dto.Id,
            dto.Name,
            dto.Key,
            dto.Description);
    }

    public static GameShortResponse AsShortResponse(this GameDto dto)
    {
        return new GameShortResponse(
            dto.Id,
            dto.Name);
    }

    public static CreateGameDto AsDto(this CreateGameRequest request)
    {
        return new CreateGameDto(
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.Genres,
            request.Platforms);
    }

    public static UpdateGameDto AsDto(this UpdateGameRequest request)
    {
        return new UpdateGameDto(
            request.Game.Id,
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.GenresIds,
            request.PlatformsIds);
    }
}