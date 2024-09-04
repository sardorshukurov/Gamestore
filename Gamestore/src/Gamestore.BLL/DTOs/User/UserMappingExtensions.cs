using UserEntity = Gamestore.Domain.Entities.Users.User;

namespace Gamestore.BLL.DTOs.User;

public static class UserMappingExtensions
{
    public static UserResponse ToResponse(this UserEntity entity)
    {
        return new UserResponse(
            entity.Id,
            entity.FirstName ?? string.Empty);
    }
}
