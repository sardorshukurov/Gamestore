using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Games;

public class Platform : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Type { get; set; }
}