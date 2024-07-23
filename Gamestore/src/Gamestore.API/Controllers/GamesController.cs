using System.Text;
using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.API.DTOs.Platform;
using Gamestore.API.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.OrderService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController(
    IGameService gameService,
    IGenreService genreService,
    IPlatformService platformService,
    IPublisherService publisherService,
    IOrderService orderService) : ControllerBase
{
    // TODO: what is this used for?
    // either use nullable types or generate new values, there should be no hardcoded values
    private readonly Guid _customerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameRequest request)
    {
        await gameService.CreateAsync(request.ToDto());
        return Ok();
    }

    [HttpGet("{key}/key")]
    public async Task<ActionResult<GameResponse>> GetByKey(string key)
    {
        var game = await gameService.GetByKeyAsync(key);

        return game is null ? NotFound($"Game with key {key} not found") : Ok(game.ToResponse());
    }

    [HttpGet("{genreId}/genre")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGamesByGenre(Guid genreId)
    {
        var games = (await gameService.GetByGenreAsync(genreId))
            .Select(g => g.ToResponse());

        return Ok(games);
    }

    [HttpGet("{platformId}/platform")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGamesByPlatform(Guid platformId)
    {
        var games = (await gameService.GetByPlatformAsync(platformId))
            .Select(g => g.ToResponse());

        return Ok(games);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GameResponse>> GetById(Guid id)
    {
        var game = await gameService.GetByIdAsync(id);

        return game is null ? NotFound($"Game with id {id} not found") : Ok(game.ToResponse());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll()
    {
        var games = (await gameService.GetAllAsync())
            .Select(g => g.ToResponse());
        return Ok(games);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGameRequest request)
    {
        try
        {
            var gameDto = request.ToDto();

            await gameService.UpdateAsync(gameDto);
            return NoContent();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        try
        {
            await gameService.DeleteByKeyAsync(key);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Game with key {key} not found");
        }
    }

    [HttpGet("{key}/file")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetFile(string key)
    {
        var game = await gameService.GetByKeyAsync(key);

        if (game is null)
        {
            return NotFound($"Game with key {key} not found");
        }

        var fileName = $"{game.Name}_{DateTime.Now:yyyyMMddHHmmss}.txt";

        // serialize game object to return as txt file
        var serializedGame = JsonConvert.SerializeObject(game);

        return File(Encoding.UTF8.GetBytes(serializedGame), "text/plain", fileName);
    }

    [HttpGet("{key}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetGenresByKey(string key)
    {
        try
        {
            var genres = (await genreService.GetAllByGameKeyAsync(key))
                .Select(g => g.ToShortResponse());

            return Ok(genres);
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpGet("{key}/platforms")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetPlatformsByKey(string key)
    {
        try
        {
            var platforms = (await platformService.GetAllByGameKeyAsync(key))
                .Select(p => p.ToShortResponse());

            return Ok(platforms);
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpGet("{key}/publisher")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PublisherResponse>> GetPublisherByKey(string key)
    {
        try
        {
            var publisher = await publisherService.GetByGameKeyAsync(key);

            return publisher is null
                ? NotFound($"Publisher for the game with game key {key} not found")
                : Ok(publisher.ToResponse());
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpPost("{key}/buy")]
    public async Task<IActionResult> BuyGame(string key)
    {
        try
        {
            await orderService.AddGameInTheCartAsync(_customerId, key);
            return Ok();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
        catch (NotEnoughGamesInStockException negex)
        {
            return BadRequest(negex.Message);
        }
    }
}
