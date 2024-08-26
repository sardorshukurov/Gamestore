using NorthwindOrderEntity = Gamestore.Domain.Entities.Northwind.Order;
using OrderEntity = Gamestore.Domain.Entities.Order;
using OrderGameEntity = Gamestore.Domain.Entities.OrderGame;

namespace Gamestore.BLL.DTOs.Order;

public static class OrderMappingExtensions
{
    public static OrderResponse ToResponse(this OrderEntity entity)
    {
        return new OrderResponse(
            entity.Id.ToString(),
            entity.CustomerId.ToString(),
            entity.Date,
            OrderSource.GameStore);
    }

    public static OrderResponse ToResponse(this NorthwindOrderEntity entity)
    {
        return new OrderResponse(
            entity.OrderId.ToString(),
            entity.CustomerId.ToString(),
            entity.OrderDate,
            OrderSource.Northwind);
    }

    public static OrderDetailsResponse ToResponse(this OrderGameEntity entity)
    {
        return new OrderDetailsResponse(
            entity.ProductId,
            entity.Price,
            entity.Quantity,
            entity.Discount);
    }
}