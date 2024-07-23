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
            dto.Description,
            dto.Price,
            dto.Discount,
            dto.UnitInStock);
    }

    public static GameShortResponse AsShortResponse(this GameDto dto)
    {
        return new GameShortResponse(
            dto.Id,
            dto.Name);
    }

    // TODO: better naming `ToDto`
    public static CreateGameDto AsDto(this CreateGameRequest request)
    {
        return new CreateGameDto(
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.Game.Price,
            request.Game.UnitInStock,
            request.Game.Discount,
            request.Genres,
            request.Platforms,
            request.Publisher);
    }

    public static UpdateGameDto AsDto(this UpdateGameRequest request)
    {
        return new UpdateGameDto(
            request.Game.Id,
            request.Game.Name,
            request.Game.Key,
            request.Game.Description,
            request.Game.Price,
            request.Game.UnitInStock,
            request.Game.Discount,
            request.Genres,
            request.Platforms,
            request.Publisher);
    }
}