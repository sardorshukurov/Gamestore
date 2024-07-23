using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
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
        await platformService.CreateAsync(request.ToDto());
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PlatformShortResponse>> GetById(Guid id)
    {
        var platform = await platformService.GetByIdAsync(id);

        return platform is null ? NotFound($"Platform with id {id} not found") : Ok(platform.ToShortResponse());
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetAll()
    {
        var platforms = (await platformService.GetAllAsync())
            .Select(p => p.ToShortResponse());

        return Ok(platforms);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePlatformRequest request)
    {
        try
        {
            await platformService.UpdateAsync(request.ToDto());

            return NoContent();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await platformService.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Platform with id {id} not found");
        }
    }
}
