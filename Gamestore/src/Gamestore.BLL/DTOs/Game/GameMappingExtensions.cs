using GameEntity = Gamestore.Domain.Entities.Game;

namespace Gamestore.BLL.DTOs.Game;

public static class GameMappingExtensions
{
    public static GameDto ToDto(this GameEntity game)
    {
        return new GameDto(
            game.Id,
            game.Name,
            game.Key,
            game.Description,
            game.Price,
            game.Discount,
            game.UnitInStock);
    }

    public static GameShortDto ToShortDto(this GameEntity game)
    {
        return new GameShortDto(
            game.Id,
            game.Name);
    }

    public static GameEntity ToEntity(this CreateGameDto dto)
    {
        return new GameEntity
        {
            Name = dto.Name,
            Key = dto.Key ?? dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            UnitInStock = dto.UnitInStock,
            Discount = dto.Discount,
            PublisherId = dto.PublisherId,
        };
    }

    public static void UpdateEntity(this UpdateGameDto dto, GameEntity game)
    {
        game.Name = dto.Name;
        game.Key = dto.Key ?? dto.Name;
        game.Description = dto.Description;
        game.Price = dto.Price;
        game.UnitInStock = dto.UnitInStock;
        game.Discount = dto.Discount;
        game.PublisherId = dto.PublisherId;
    }
}