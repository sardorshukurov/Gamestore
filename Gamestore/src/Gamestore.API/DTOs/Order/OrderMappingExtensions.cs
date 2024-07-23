using Gamestore.BLL.DTOs.Order;

namespace Gamestore.API.DTOs.Order;

public static class OrderMappingExtensions
{
    public static OrderResponse ToResponse(this OrderDto dto)
    {
        return new OrderResponse(
            dto.Id,
            dto.CustomerId,
            dto.Date);
    }

    public static OrderDetailsResponse ToResponse(this OrderDetailsDto dto)
    {
        return new OrderDetailsResponse(
            dto.ProductId,
            dto.Price,
            dto.Quantity,
            dto.Discount);
    }
}