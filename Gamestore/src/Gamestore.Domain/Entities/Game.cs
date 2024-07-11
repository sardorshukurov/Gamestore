using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class Game
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Key { get; set; }

    public string? Description { get; set; }
}