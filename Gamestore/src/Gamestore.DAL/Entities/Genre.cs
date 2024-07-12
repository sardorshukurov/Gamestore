using System.ComponentModel.DataAnnotations;
using Gamestore.DAL.Common;

namespace Gamestore.DAL.Entities;

public class Genre : IBaseEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}