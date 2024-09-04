using PaymentMethodEntity = Gamestore.Domain.Entities.Orders.PaymentMethod;

namespace Gamestore.BLL.DTOs.Order.Payment;

public static class PaymentMappingExtensions
{
    public static PaymentMethodResponse ToResponse(this PaymentMethodEntity entity)
    {
        return new PaymentMethodResponse(
            entity.ImageUrl,
            entity.Title,
            entity.Description);
    }
}