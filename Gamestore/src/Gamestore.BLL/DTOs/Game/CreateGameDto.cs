namespace Gamestore.BLL.DTOs.Game;

public record CreateGameDto(
    string Name,
    string? Key,
    string? Description,
    double Price,
    int UnitInStock,
    int Discount,
    ICollection<Guid> GenresIds,
    ICollection<Guid> PlatformsIds,
    Guid PublisherId);