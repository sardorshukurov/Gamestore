using Gamestore.BLL.DTOs.User;
using Gamestore.BLL.Services.UserService;
using Gamestore.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetRoles()
    => Ok(await userService.GetRolesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<UserRoleResponse>> GetRole(Guid id)
        => Ok(await userService.GetRoleByIdAsync(id));

    [HttpGet("permissions")]
    public ActionResult<IEnumerable<Permissions>> GetPermissions()
        => Ok(Enum.GetValues(typeof(Permissions)));

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserRoleResponse>> DeleteRole(Guid id)
    {
        await userService.DeleteRoleAsync(id);
        return NoContent();
    }
}
