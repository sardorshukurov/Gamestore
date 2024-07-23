using System.ComponentModel.DataAnnotations;
using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Genre : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}