using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Platform : IBaseEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Type { get; set; }
}