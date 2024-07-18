using System.ComponentModel.DataAnnotations;

namespace Gamestore.DAL.Entities;

public class GamePlatform
{
    [Key]
    public Guid GameId { get; set; }

    [Key]
    public Guid PlatformId { get; set; }
}