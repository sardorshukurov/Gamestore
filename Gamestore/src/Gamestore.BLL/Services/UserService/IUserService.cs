using Gamestore.BLL.DTOs.User;

namespace Gamestore.BLL.Services.UserService;

public interface IUserService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);

    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task DeleteUserAsync(Guid userId);
}