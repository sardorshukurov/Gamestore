using System.ComponentModel.DataAnnotations;
using Gamestore.DAL.Common;

namespace Gamestore.DAL.Entities;

public class Game : IBaseEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; }

    [Required]
    public string Key { get; set; }

    public string? Description { get; set; }
}