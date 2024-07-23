using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class GameGenre
{
    [Key]
    public Guid GameId { get; set; }

    [Key]
    public Guid GenreId { get; set; }
}