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
    CreateGameValidator createValidator,
    UpdateGameValidator updateValidator,
    IOrderService orderService) : ControllerBase
{
    private readonly Guid _customerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameRequest request)
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

            await gameService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<GameResponse>> GetByKey(string key)
    {
        try
        {
            var game = await gameService.GetByKeyAsync(key);

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
            var game = await gameService.GetByIdAsync(id);

            return game is null ? NotFound($"Game with id {id} not found") : Ok(game.AsResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll()
    {
        try
        {
            var games = (await gameService.GetAllAsync())
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
            var result = await updateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                });
            }

            var gameDto = request.AsDto();

            await gameService.UpdateAsync(gameDto);
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
            await gameService.DeleteByKeyAsync(key);
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
            var genres = (await genreService.GetAllByGameKeyAsync(key))
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
            var platforms = (await platformService.GetAllByGameKeyAsync(key))
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

    [HttpGet("{key}/publisher")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PublisherResponse>> GetPublisherByKey(string key)
    {
        try
        {
            var publisher = await publisherService.GetByGameKeyAsync(key);

            return publisher is null
                ? NotFound($"Publisher for the game with game key {key} not found")
                : Ok(publisher.AsResponse());
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
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }
}
