using Gamestore.BLL.DTOs.User;
using Gamestore.BLL.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        => Ok(await userService.Login(request));
}
