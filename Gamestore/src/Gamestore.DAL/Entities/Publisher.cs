using System.ComponentModel.DataAnnotations;
using Gamestore.DAL.Common;

namespace Gamestore.DAL.Entities;

public class Publisher : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CompanyName { get; set; }

    public string? HomePage { get; set; }

    public string? Description { get; set; }
}