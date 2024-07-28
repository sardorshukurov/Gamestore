using System.Text;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.DTOs.Platform;
using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.OrderService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.BLL.Services.PublisherService;
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
        await gameService.CreateAsync(request);
        return Ok();
    }

    [HttpGet("{key}/key")]
    public async Task<ActionResult<GameResponse>> GetByKey(string key)
    {
        var game = await gameService.GetByKeyAsync(key);

        return game is null ? NotFound($"Game with key {key} not found") : Ok(game);
    }

    [HttpGet("{genreId}/genre")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGamesByGenre(Guid genreId)
    {
        var games = await gameService.GetByGenreAsync(genreId);

        return Ok(games);
    }

    [HttpGet("{platformId}/platform")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGamesByPlatform(Guid platformId)
    {
        var games = await gameService.GetByPlatformAsync(platformId);
        return Ok(games);
    }

    [HttpGet("{companyName}/companyName")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGamesByPublisher(string companyName)
    {
        var games = await gameService.GetByPublisherAsync(companyName);

        return Ok(games);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GameResponse>> GetById(Guid id)
    {
        var game = await gameService.GetByIdAsync(id);

        return game is null ? NotFound($"Game with id {id} not found") : Ok(game);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll()
    {
        var games = await gameService.GetAllAsync();
        return Ok(games);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGameRequest request)
    {
        var gameDto = request;

        await gameService.UpdateAsync(gameDto);
        return NoContent();
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        await gameService.DeleteByKeyAsync(key);
        return NoContent();
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
        var genres = await genreService.GetAllByGameKeyAsync(key);

        return Ok(genres);
    }

    [HttpGet("{key}/platforms")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PlatformShortResponse>>> GetPlatformsByKey(string key)
    {
        var platforms = await platformService.GetAllByGameKeyAsync(key);

        return Ok(platforms);
    }

    [HttpGet("{key}/publisher")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PublisherResponse>> GetPublisherByKey(string key)
    {
        var publisher = await publisherService.GetByGameKeyAsync(key);

        return publisher is null
            ? NotFound($"Publisher for the game with game key {key} not found")
            : Ok(publisher);
    }

    [HttpPost("{key}/buy")]
    public async Task<IActionResult> BuyGame(string key)
    {
        await orderService.AddGameInTheCartAsync(_customerId, key);
        return Ok();
    }
}
