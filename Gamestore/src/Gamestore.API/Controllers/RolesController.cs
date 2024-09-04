using Gamestore.BLL.DTOs.Role;
using Gamestore.BLL.Services.RoleService;
using Gamestore.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetRoles()
    => Ok(await roleService.GetRolesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<UserRoleResponse>> GetRole(Guid id)
        => Ok(await roleService.GetRoleByIdAsync(id));

    [HttpGet("permissions")]
    public ActionResult<IEnumerable<Permissions>> GetPermissions()
        => Ok(Enum.GetValues(typeof(Permissions)));

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserRoleResponse>> DeleteRole(Guid id)
    {
        await roleService.DeleteRoleAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/permissions")]
    public async Task<ActionResult<Permissions>> GetPermissions(Guid id)
        => await roleService.GetRolePermissionsAsync(id);

    [HttpPost]
    public async Task<IActionResult> Post(CreateRoleRequest request)
    {
        await roleService.AddRoleAsync(request);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateRoleRequest request)
    {
        await roleService.UpdateRoleAsync(request);
        return NoContent();
    }
}
