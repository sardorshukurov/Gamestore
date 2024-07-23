using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(
    IPlatformService platformService,
    CreatePlatformValidator createValidator,
    UpdatePlatformValidator updateValidator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreatePlatformRequest request)
    {
        var result = await createValidator.ValidateAsync(request);

        if (!result.IsValid)
        {
            return BadRequest(new
            {
                message = "Validation failed",
                errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
            });
        }

        await platformService.CreateAsync(request.AsDto());
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PlatformShortResponse>> GetById(Guid id)
    {
        var platform = await platformService.GetByIdAsync(id);

        return platform is null ? NotFound($"Platform with id {id} not found") : Ok(platform.AsShortResponse());
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetAll()
    {
        var platforms = (await platformService.GetAllAsync())
            .Select(p => p.AsShortResponse());

        return Ok(platforms);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePlatformRequest request)
    {
        try
        {
            var result = await updateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                });
            }

            await platformService.UpdateAsync(request.AsDto());

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
