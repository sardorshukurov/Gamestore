using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Publisher : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CompanyName { get; set; }

    public string? HomePage { get; set; }

    public string? Description { get; set; }
}