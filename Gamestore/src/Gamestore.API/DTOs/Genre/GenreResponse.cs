namespace Gamestore.API.DTOs.Genre;

public record GenreResponse(
    Guid Id,
    string Name,
    Guid? ParentGenreId);