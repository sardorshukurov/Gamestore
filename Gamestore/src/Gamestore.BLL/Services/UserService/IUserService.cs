using Gamestore.BLL.DTOs.Role;
using Gamestore.BLL.DTOs.User;

namespace Gamestore.BLL.Services.UserService;

public interface IUserService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);

    Task RegisterAsync(RegisterUserRequest request);

    Task UpdateUserAsync(UpdateUserRequest request);

    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task DeleteUserAsync(Guid userId);

    Task<IEnumerable<UserRoleResponse>> GetUserRolesAsync(Guid userId);
}