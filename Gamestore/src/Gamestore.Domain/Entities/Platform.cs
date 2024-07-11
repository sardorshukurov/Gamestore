using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class Platform
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Type { get; set; }
}