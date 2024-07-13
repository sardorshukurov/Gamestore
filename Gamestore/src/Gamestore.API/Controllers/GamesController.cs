using Gamestore.API.DTOs.Game;
using Gamestore.BLL.Services.GameService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController(IGameService gameService) : ControllerBase
{
    private readonly IGameService _gameService = gameService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameShortResponse>>> GetAll()
    {
        try
        {
            return Ok(await _gameService.GetAllAsync());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
