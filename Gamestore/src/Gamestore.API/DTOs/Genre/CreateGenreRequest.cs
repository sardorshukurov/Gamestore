namespace Gamestore.API.DTOs.Genre;

public record CreateGenreRequest(
    string Name,
    Guid? ParentGenreId);