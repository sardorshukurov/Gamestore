using OrderEntity = Gamestore.DAL.Entities.Order;
using OrderGameEntity = Gamestore.DAL.Entities.OrderGame;

namespace Gamestore.BLL.DTOs.Order;

public static class OrderMappingExtensions
{
    public static OrderDto AsDto(this OrderEntity order)
    {
        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Date);
    }

    public static OrderDetailsDto AsDto(this OrderGameEntity orderGame)
    {
        return new OrderDetailsDto(
            orderGame.ProductId,
            orderGame.Price,
            orderGame.Quantity,
            orderGame.Discount);
    }
}