using System.ComponentModel.DataAnnotations;

namespace Gamestore.DAL.Entities;

public class GameGenre
{
    [Key]
    [Required]
    public Guid GameId { get; set; }

    [Key]
    [Required]
    public Guid GenreId { get; set; }
}