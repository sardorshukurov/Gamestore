using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class GameGenre
{
    [Key]
    [Required]
    public Guid GameId { get; set; }

    [Key]
    [Required]
    public Guid GenreId { get; set; }
}