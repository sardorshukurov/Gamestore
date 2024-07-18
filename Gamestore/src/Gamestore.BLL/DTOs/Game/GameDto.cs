namespace Gamestore.BLL.DTOs.Game;

public record GameDto(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    double Price,
    int Discount,
    int UnitInStock);