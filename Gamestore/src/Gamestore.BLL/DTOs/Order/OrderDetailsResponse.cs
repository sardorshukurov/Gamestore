namespace Gamestore.BLL.DTOs.Order;

public record OrderDetailsResponse(
    Guid ProductId,
    double Price,
    int Quantity,
    int Discount);