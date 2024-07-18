namespace Gamestore.BLL.DTOs.Game;

public record UpdateGameDto(
    Guid Id,
    string Name,
    string? Key,
    string? Description,
    double Price,
    int UnitInStock,
    int Discount,
    ICollection<Guid> GenresIds,
    ICollection<Guid> PlatformsIds,
    Guid PublisherId);