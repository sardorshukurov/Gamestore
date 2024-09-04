using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Users;

public class User : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public virtual ICollection<UserRole> Roles { get; set; } =
        [];
}