using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Users;

public class UserRole : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public Permissions Permissions { get; set; }
}
