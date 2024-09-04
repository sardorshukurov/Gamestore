using Gamestore.BLL.DTOs.User;
using Gamestore.BLL.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
        => Ok(await userService.LoginAsync(request));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> Get()
        => Ok(await userService.GetAllAsync());

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetRoles()
        => Ok(await userService.GetRolesAsync());

    [HttpGet("roles/{id}")]
    public async Task<ActionResult<UserRoleResponse>> GetRole(Guid id)
        => Ok(await userService.GetRoleByIdAsync(id));

    [HttpDelete("roles/{id}")]
    public async Task<ActionResult<UserRoleResponse>> DeleteRole(Guid id)
    {
        await userService.DeleteRoleAsync(id);
        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Post(RegisterUserRequest request)
    {
        await userService.RegisterAsync(request);
        return Ok();
    }
}
