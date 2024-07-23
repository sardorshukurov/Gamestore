using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Game : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string Key { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public int UnitInStock { get; set; }

    public int Discount { get; set; }

    public Guid PublisherId { get; set; }
}