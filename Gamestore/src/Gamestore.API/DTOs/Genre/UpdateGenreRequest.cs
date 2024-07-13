namespace Gamestore.API.DTOs.Genre;

public record UpdateGenreRequest(
    Guid Id,
    string Name,
    Guid? ParentGenreId);