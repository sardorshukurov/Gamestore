using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PlatformsController(IPlatformService platformService, IGameService gameService) : ControllerBase
{
    private readonly IPlatformService _platformService = platformService;
    private readonly IGameService _gameService = gameService;

    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGames(Guid id)
    {
        try
        {
            var games = (await _gameService.GetByPlatformAsync(id))
                .Select(g => g.AsResponse());

            return Ok(games);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePlatformRequest request)
    {
        try
        {
            await _platformService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PlatformShortResponse>> GetById(Guid id)
    {
        try
        {
            var platform = await _platformService.GetByIdAsync(id);

            return platform is null ? NotFound($"Platform with id {id} not found") : Ok(platform.AsShortResponse());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetAll()
    {
        try
        {
            var platforms = (await _platformService.GetAllAsync())
                .Select(p => p.AsShortResponse());

            return Ok(platforms);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePlatformRequest request)
    {
        try
        {
            await _platformService.UpdateAsync(request.AsDto());

            return NoContent();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _platformService.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Platform with id {id} not found");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
