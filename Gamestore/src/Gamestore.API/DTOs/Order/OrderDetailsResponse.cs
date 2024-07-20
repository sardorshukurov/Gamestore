namespace Gamestore.API.DTOs.Order;

public record OrderDetailsResponse(
    Guid ProductId,
    double Price,
    int Quantity,
    int Discount);