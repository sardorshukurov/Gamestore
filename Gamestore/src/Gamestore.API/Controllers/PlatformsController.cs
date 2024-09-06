using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.DAL.Data.Seeder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(
    IPlatformService platformService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
    public async Task<IActionResult> Create(CreatePlatformRequest request)
    {
        await platformService.CreateAsync(request);
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PlatformShortResponse>> GetById(Guid id)
    {
        var platform = await platformService.GetByIdAsync(id);

        return platform is null ? NotFound($"Platform with id {id} not found") : Ok(platform);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetAll()
    {
        var platforms = await platformService.GetAllAsync();

        return Ok(platforms);
    }

    [HttpGet("{gameKey}/game")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetPlatformsByGameKey(string gameKey)
    {
        var platforms = await platformService.GetAllByGameKeyAsync(gameKey);

        return Ok(platforms);
    }

    [HttpPut]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
    public async Task<IActionResult> Update(UpdatePlatformRequest request)
    {
        await platformService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await platformService.DeleteAsync(id);

        return NoContent();
    }
}
