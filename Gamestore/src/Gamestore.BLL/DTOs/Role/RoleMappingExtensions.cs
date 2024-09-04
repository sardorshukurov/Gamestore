using UserRoleEntity = Gamestore.Domain.Entities.Users.UserRole;

namespace Gamestore.BLL.DTOs.Role;

public static class RoleMappingExtensions
{
    public static UserRoleResponse ToResponse(this UserRoleEntity entity)
    {
        return new UserRoleResponse(
            entity.Id,
            entity.Name ?? string.Empty);
    }

    public static UserRoleEntity ToEntity(this CreateRoleRequest request)
    {
        return new UserRoleEntity
        {
            Name = request.Name,
            Permissions = request.Permissions,
        };
    }
}
