using System.Text;
using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.API.DTOs.Platform;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController(IGameService gameService, IGenreService genreService, IPlatformService platformService) : ControllerBase
{
    private readonly IGameService _gameService = gameService;
    private readonly IGenreService _genreService = genreService;
    private readonly IPlatformService _platformService = platformService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameRequest request)
    {
        try
        {
            await _gameService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{key}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GameResponse>> GetByKey(string key)
    {
        try
        {
            var game = await _gameService.GetByKeyAsync(key);

            return game is null ? NotFound($"Game with key {key} not found") : Ok(game.AsResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("find/{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GameResponse>> GetById(Guid id)
    {
        try
        {
            var game = await _gameService.GetByIdAsync(id);

            return game is null ? NotFound($"Game with id {id} not found") : Ok(game.AsResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll()
    {
        try
        {
            var games = (await _gameService.GetAllAsync())
                .Select(g => g.AsResponse());
            return Ok(games);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGameRequest request)
    {
        try
        {
            var gameDto = request.AsDto();

            await _gameService.UpdateAsync(gameDto);
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

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        try
        {
            await _gameService.DeleteByKeyAsync(key);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Game with key {key} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{key}/file")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetFile(string key)
    {
        try
        {
            var game = await _gameService.GetByKeyAsync(key);

            if (game is null)
            {
                return NotFound($"Game with key {key} not found");
            }

            var fileName = $"{game.Name}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var serializedGame = JsonConvert.SerializeObject(game);
            return File(Encoding.UTF8.GetBytes(serializedGame), "text/plain", fileName);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{key}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetGenresByKey(string key)
    {
        try
        {
            var genres = (await _genreService.GetAllByGameKeyAsync(key))
                .Select(g => g.AsShortResponse());

            return Ok(genres);
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

    [HttpGet("{key}/platforms")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetPlatformsByKey(string key)
    {
        try
        {
            var platforms = (await _platformService.GetAllByGameKeyAsync(key))
                .Select(p => p.AsShortResponse());

            return Ok(platforms);
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
}
