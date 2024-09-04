using Gamestore.BLL.DTOs.User;
using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.Services.UserService;

public interface IUserService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);

    Task RegisterAsync(RegisterUserRequest request);

    Task UpdateUserAsync(UpdateUserRequest request);

    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task DeleteUserAsync(Guid userId);

    Task<IEnumerable<UserRoleResponse>> GetRolesAsync();

    Task<IEnumerable<UserRoleResponse>> GetUserRolesAsync(Guid userId);

    Task<UserRoleResponse> GetRoleByIdAsync(Guid id);

    Task<Permissions> GetRolePermissionsAsync(Guid roleId);

    Task DeleteRoleAsync(Guid roleId);
}