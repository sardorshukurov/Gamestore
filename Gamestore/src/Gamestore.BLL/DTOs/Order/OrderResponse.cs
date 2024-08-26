namespace Gamestore.BLL.DTOs.Order;

public record OrderResponse(
    string Id,
    string CustomerId,
    DateTime? Date,
    OrderSource OrderSource);