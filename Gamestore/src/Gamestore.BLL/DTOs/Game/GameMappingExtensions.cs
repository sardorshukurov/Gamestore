using GameEntity = Gamestore.Domain.Entities.Game;

namespace Gamestore.BLL.DTOs.Game;

public static class GameMappingExtensions
{
    public static GameResponse ToResponse(this GameEntity entity)
    {
        return new GameResponse(
            entity.Id,
            entity.Name,
            entity.Key,
            entity.Description,
            entity.Price,
            entity.Discount,
            entity.UnitInStock);
    }

    public static GameShortResponse ToShortResponse(this GameEntity entity)
    {
        return new GameShortResponse(
            entity.Id,
            entity.Name);
    }

    public static GameEntity ToEntity(this CreateGameRequest request)
    {
        return new GameEntity
        {
            Name = request.Game.Name,
            Key = request.Game.Key,
            Description = request.Game.Description,
            Price = request.Game.Price,
            UnitInStock = request.Game.UnitInStock,
            Discount = request.Game.Discount,
            PublisherId = request.Publisher,
        };
    }

    public static void UpdateEntity(this UpdateGameRequest request, GameEntity entity)
    {
        entity.Name = request.Game.Name;
        entity.Key = request.Game.Key;
        entity.Description = request.Game.Description;
        entity.Price = request.Game.Price;
        entity.UnitInStock = request.Game.UnitInStock;
        entity.Discount = request.Game.Discount;
        entity.PublisherId = request.Publisher;
    }
}