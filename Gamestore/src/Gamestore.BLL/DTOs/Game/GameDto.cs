namespace Gamestore.BLL.DTOs.Game;

// TODO: there are some DTOs folders in the project, it's better to move all DTOs to the same folder
// and have dedicated buisness entities folder, so to move core entites from the infrastructure layer
// to the buisness layer or it's also called domain layer
public record GameDto(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    double Price,
    int Discount,
    int UnitInStock);