using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(
    IPlatformService platformService,
    IGameService gameService,
    CreatePlatformValidator createValidator,
    UpdatePlatformValidator updateValidator) : ControllerBase
{
    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGames(Guid id)
    {
        try
        {
            var games = (await gameService.GetByPlatformAsync(id))
                .Select(g => g.AsResponse());

            return Ok(games);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePlatformRequest request)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PlatformShortResponse>> GetById(Guid id)
    {
        try
        {
            var platform = await platformService.GetByIdAsync(id);

            return platform is null ? NotFound($"Platform with id {id} not found") : Ok(platform.AsShortResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetAll()
    {
        try
        {
            var platforms = (await platformService.GetAllAsync())
                .Select(p => p.AsShortResponse());

            return Ok(platforms);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }
}
