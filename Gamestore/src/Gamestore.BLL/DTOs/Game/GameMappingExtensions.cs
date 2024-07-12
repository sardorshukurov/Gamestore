using GameEntity = Gamestore.DAL.Entities.Game;

namespace Gamestore.BLL.DTOs.Game;

public static class GameMappingExtensions
{
    public static GameDto AsDto(this GameEntity game)
    {
        return new GameDto(
            game.Id,
            game.Name,
            game.Key,
            game.Description);
    }

    public static GameShortDto AsShortDto(this GameEntity game)
    {
        return new GameShortDto(
            game.Id,
            game.Name);
    }

    public static GameEntity AsEntity(this CreateGameDto dto)
    {
        return new GameEntity
        {
            Name = dto.Name,
            Key = dto.Key,
            Description = dto.Description,
        };
    }

    public static void UpdateEntity(this UpdateGameDto dto, GameEntity game)
    {
        game.Name = dto.Name;
        game.Key = dto.Key;
        game.Description = dto.Description;
    }
}