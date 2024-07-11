using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class Genre
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}