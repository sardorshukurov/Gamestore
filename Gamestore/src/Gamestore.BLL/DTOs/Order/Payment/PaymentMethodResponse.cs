namespace Gamestore.BLL.DTOs.Order.Payment;

public record PaymentMethodResponse(
    string ImageUrl,
    string Title,
    string Description);