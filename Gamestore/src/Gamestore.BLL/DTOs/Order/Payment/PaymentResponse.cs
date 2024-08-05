namespace Gamestore.API.DTOs.Order.Payment;

public record PaymentResponse(
    Guid UserId,
    Guid OrderId,
    DateTime PaymentDate,
    double Sum);