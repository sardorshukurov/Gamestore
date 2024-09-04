using OrderEntity = Gamestore.Domain.Entities.Orders.Order;
using OrderGameEntity = Gamestore.Domain.Entities.Orders.OrderGame;

namespace Gamestore.BLL.DTOs.Order;

public static class OrderMappingExtensions
{
    public static OrderResponse ToResponse(this OrderEntity entity)
    {
        return new OrderResponse(
            entity.Id,
            entity.CustomerId,
            entity.Date);
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