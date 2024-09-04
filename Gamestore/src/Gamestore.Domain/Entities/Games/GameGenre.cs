namespace Gamestore.Domain.Entities.Games;

public class GameGenre
{
    public Guid GameId { get; set; }

    public virtual Game Game { get; set; }

    public Guid GenreId { get; set; }

    public virtual Genre Genre { get; set; }
}