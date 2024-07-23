using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Platform : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Type { get; set; }
}