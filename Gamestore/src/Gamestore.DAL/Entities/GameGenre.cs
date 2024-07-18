using System.ComponentModel.DataAnnotations;

namespace Gamestore.DAL.Entities;

public class GameGenre
{
    [Key]
    public Guid GameId { get; set; }

    [Key]
    public Guid GenreId { get; set; }
}