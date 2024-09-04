using Gamestore.BLL.DTOs.User;

namespace Gamestore.BLL.Services.UserService;

public interface IUserService
{
    Task<AuthResponse> Login(AuthRequest request);
}