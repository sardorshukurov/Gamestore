using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Users;

public class UserRole : IBaseEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<Permissions> Permissions { get; set; } =
        [];
}
