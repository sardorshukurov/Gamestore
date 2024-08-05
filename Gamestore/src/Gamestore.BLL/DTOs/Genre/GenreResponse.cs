namespace Gamestore.BLL.DTOs.Genre;

public record GenreResponse(
    Guid Id,
    string Name,
    Guid? ParentGenreId);