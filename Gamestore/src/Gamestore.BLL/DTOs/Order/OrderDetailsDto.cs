namespace Gamestore.BLL.DTOs.Order;

public record OrderDetailsDto(
    Guid ProductId,
    double Price,
    int Quantity,
    int Discount);