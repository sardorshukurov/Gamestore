using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController(IGenreService genreService, IGameService gameService) : ControllerBase
{
    private readonly IGenreService _genreService = genreService;
    private readonly IGameService _gameService = gameService;

    [HttpGet("{id}/games")]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGames(Guid id)
    {
        try
        {
            var games = (await _gameService.GetByGenreAsync(id))
                .Select(g => g.AsResponse());

            return Ok(games);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreRequest request)
    {
        try
        {
            await _genreService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreShortResponse>> GetGenre(Guid id)
    {
        try
        {
            var genre = await _genreService.GetByIdAsync(id);

            return genre is null ? NotFound($"Genre with id {id} not found") : Ok(genre.AsShortResponse());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetAll()
    {
        try
        {
            var genres = (await _genreService.GetAllAsync())
                .Select(g => g.AsShortResponse());

            return Ok(genres);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{parentId}/genres")]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetSubGenres(Guid parentId)
    {
        try
        {
            var genres = (await _genreService.GetSubGenresAsync(parentId))
                .Select(g => g.AsShortResponse());

            return Ok(genres);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
    {
        try
        {
            await _genreService.UpdateAsync(request.AsDto());

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
            await _genreService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Genre with id {id} not found");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
