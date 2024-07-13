using Gamestore.API.DTOs.Game;

namespace Gamestore.API.DTOs.Genre;

public record GenreResponse(
    Guid Id,
    string Name,
    string? ParentGenreName,
    ICollection<GameShortResponse> Games);