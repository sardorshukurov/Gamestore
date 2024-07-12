using System.ComponentModel.DataAnnotations;

namespace Gamestore.DAL.Entities;

public class GamePlatform
{
    [Key]
    [Required]
    public Guid GameId { get; set; }

    [Key]
    [Required]
    public Guid PlatformId { get; set; }
}