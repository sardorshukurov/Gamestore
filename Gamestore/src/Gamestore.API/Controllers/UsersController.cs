using Gamestore.BLL.DTOs.Role;
using Gamestore.BLL.DTOs.User;
using Gamestore.BLL.Services.UserService;
using Gamestore.DAL.Data.Seeder;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{id}/roles")]
    public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetUserRoles(Guid id)
        => Ok(await userService.GetUserRolesAsync(id));

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Post(RegisterUserRequest request)
    {
        await userService.RegisterAsync(request);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}")]
    public async Task<IActionResult> Put(UpdateUserRequest request)
    {
        await userService.UpdateUserAsync(request);
        return NoContent();
    }
}
