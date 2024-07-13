using Gamestore.BLL.DTOs.Game;

namespace Gamestore.BLL.DTOs.Genre;

public record GenreDto(
    Guid Id,
    string Name,
    Guid? ParentGenreId,
    string? ParentGenreName,
    ICollection<GameShortDto> Games);