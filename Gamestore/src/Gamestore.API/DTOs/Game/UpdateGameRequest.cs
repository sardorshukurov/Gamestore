namespace Gamestore.API.DTOs.Game;

public record UpdateGameRequest(
    UpdateGame Game,
    ICollection<Guid> Genres,
    ICollection<Guid> Platforms,
    Guid Publisher);

public record UpdateGame(
    Guid Id,
    string Name,
    string Key,
    string Description,
    double Price,
    int UnitInStock,
    int Discount);