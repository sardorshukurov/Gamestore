namespace Gamestore.BLL.DTOs.Game;

public record GamesResponse(
    ICollection<GameResponse> Games,
    int TotalPages,
    int CurrentPage);