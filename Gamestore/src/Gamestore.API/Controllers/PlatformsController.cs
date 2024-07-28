using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(
    IPlatformService platformService) : ControllerBase
{
    [HttpPost]
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

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePlatformRequest request)
    {
        await platformService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await platformService.DeleteAsync(id);

        return NoContent();
    }
}
