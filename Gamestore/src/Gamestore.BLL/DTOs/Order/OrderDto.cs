namespace Gamestore.BLL.DTOs.Order;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    DateTime Date);