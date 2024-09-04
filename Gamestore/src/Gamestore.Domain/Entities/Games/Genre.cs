using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Games;

public class Genre : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }
}