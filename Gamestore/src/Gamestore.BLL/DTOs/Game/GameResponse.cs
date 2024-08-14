namespace Gamestore.BLL.DTOs.Game;

public record GameResponse(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    double Price,
    float Discount,
    int UnitInStock);