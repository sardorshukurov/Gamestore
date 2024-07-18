using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController(IGenreService genreService, IGameService gameService, CreateGenreValidator createValidator, UpdateGenreValidator updateValidator) : ControllerBase
{
    private readonly IGenreService _genreService = genreService;
    private readonly IGameService _gameService = gameService;

    private readonly CreateGenreValidator _createValidator = createValidator;
    private readonly UpdateGenreValidator _updateValidator = updateValidator;

    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGames(Guid id)
    {
        try
        {
            var games = (await _gameService.GetByGenreAsync(id))
                .Select(g => g.AsResponse());

            return Ok(games);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreRequest request)
    {
        try
        {
            var result = await _createValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                });
            }

            await _genreService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GenreShortResponse>> GetGenre(Guid id)
    {
        try
        {
            var genre = await _genreService.GetByIdAsync(id);

            return genre is null ? NotFound($"Genre with id {id} not found") : Ok(genre.AsShortResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetAll()
    {
        try
        {
            var genres = (await _genreService.GetAllAsync())
                .Select(g => g.AsShortResponse());

            return Ok(genres);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{parentId}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetSubGenres(Guid parentId)
    {
        try
        {
            var genres = (await _genreService.GetSubGenresAsync(parentId))
                .Select(g => g.AsShortResponse());

            return Ok(genres);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
    {
        try
        {
            var result = await _updateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                });
            }

            await _genreService.UpdateAsync(request.AsDto());

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
            await _genreService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Genre with id {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }
}
