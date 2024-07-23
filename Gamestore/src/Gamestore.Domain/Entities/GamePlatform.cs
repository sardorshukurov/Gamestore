using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Entities;

public class GamePlatform
{
    [Key]
    public Guid GameId { get; set; }

    [Key]
    public Guid PlatformId { get; set; }
}