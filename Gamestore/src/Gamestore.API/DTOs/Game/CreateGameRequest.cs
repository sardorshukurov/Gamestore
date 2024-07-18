namespace Gamestore.API.DTOs.Game;

public record CreateGameRequest(
    CreateGame Game,
    ICollection<Guid> Genres,
    ICollection<Guid> Platforms,
    Guid Publisher);

public record CreateGame(
    string Name,
    string Key,
    string Description,
    double Price,
    int UnitInStock,
    int Discount);