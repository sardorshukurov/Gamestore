using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController(
    IGenreService genreService,
    IGameService gameService,
    CreateGenreValidator createValidator,
    UpdateGenreValidator updateValidator) : ControllerBase
{
    // TODO: duplicate method it already exists in the GamesController
    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetAllGames(Guid id)
    {
        var games = (await gameService.GetByGenreAsync(id))
            .Select(g => g.AsResponse());

        return Ok(games);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreRequest request)
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

        await genreService.CreateAsync(request.AsDto());
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GenreShortResponse>> GetGenre(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);

        return genre is null ? NotFound($"Genre with id {id} not found") : Ok(genre.AsShortResponse());
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetAll()
    {
        var genres = (await genreService.GetAllAsync())
            .Select(g => g.AsShortResponse());

        return Ok(genres);
    }

    [HttpGet("{parentId}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetSubGenres(Guid parentId)
    {
        var genres = (await genreService.GetSubGenresAsync(parentId))
            .Select(g => g.AsShortResponse());

        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
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

            await genreService.UpdateAsync(request.AsDto());

            return NoContent();
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await genreService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Genre with id {id} not found");
        }
    }
}
