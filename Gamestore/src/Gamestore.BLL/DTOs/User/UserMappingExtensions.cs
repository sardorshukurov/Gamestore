using UserEntity = Gamestore.Domain.Entities.Users.User;
using UserRoleEntity = Gamestore.Domain.Entities.Users.UserRole;

namespace Gamestore.BLL.DTOs.User;

public static class UserMappingExtensions
{
    public static UserResponse ToResponse(this UserEntity entity)
    {
        return new UserResponse(
            entity.Id,
            entity.FirstName ?? string.Empty);
    }

    public static UserRoleResponse ToResponse(this UserRoleEntity entity)
    {
        return new UserRoleResponse(
            entity.Id,
            entity.Name ?? string.Empty);
    }
}
