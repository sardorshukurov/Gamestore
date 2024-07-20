namespace Gamestore.API.DTOs.Order;

public record OrderResponse(
    Guid Id,
    Guid CustomerId,
    DateTime? Date);