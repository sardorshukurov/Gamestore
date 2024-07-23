using Gamestore.API.DTOs.Genre;
using Gamestore.BLL.Services.GenreService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController(
    IGenreService genreService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreRequest request)
    {
        await genreService.CreateAsync(request.ToDto());
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GenreShortResponse>> GetGenre(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);

        return genre is null ? NotFound($"Genre with id {id} not found") : Ok(genre.ToShortResponse());
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetAll()
    {
        var genres = (await genreService.GetAllAsync())
            .Select(g => g.ToShortResponse());

        return Ok(genres);
    }

    [HttpGet("{parentId}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetSubGenres(Guid parentId)
    {
        var genres = (await genreService.GetSubGenresAsync(parentId))
            .Select(g => g.ToShortResponse());

        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
    {
        try
        {
            await genreService.UpdateAsync(request.ToDto());

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
