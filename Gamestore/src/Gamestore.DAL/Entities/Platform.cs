using System.ComponentModel.DataAnnotations;
using Gamestore.DAL.Common;

namespace Gamestore.DAL.Entities;

public class Platform : IBaseEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Type { get; set; }
}