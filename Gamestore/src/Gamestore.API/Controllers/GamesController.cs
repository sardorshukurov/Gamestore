using System.Text;
using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.Filtration.Games;
using Gamestore.BLL.Filtration.Games.Options;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.OrderService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GamesController(
    IGameService gameService,
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

    // TODO: fix enums mapping errors.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAll([FromQuery] SearchCriteria criteria)
    {
        var games = await gameService.GetAllAsync(criteria);
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

    [HttpPost("{key}/buy")]
    public async Task<IActionResult> BuyGame(string key)
    {
        await orderService.AddGameInTheCartAsync(_customerId, key);
        return Ok();
    }

    [HttpGet("pagination-options")]
    public ActionResult<IEnumerable<string>> GetPaginationOptions()
    {
        var paginationOptions = Enum.GetValues(typeof(PaginationOption))
            .Cast<PaginationOption>();

        return Ok(paginationOptions);
    }

    [HttpGet("sorting-options")]
    public ActionResult<IEnumerable<string>> GetSortingOptions()
    {
        var sortingOptions = Enum.GetValues(typeof(SortingOption))
            .Cast<SortingOption>();

        return Ok(sortingOptions);
    }

    [HttpGet("date-filter-options")]
    public ActionResult<IEnumerable<string>> GetDateFilterOptions()
    {
        var dateFilterOptions = Enum.GetValues(typeof(DateFilterOption))
            .Cast<DateFilterOption>();

        return Ok(dateFilterOptions);
    }
}
