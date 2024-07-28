using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.Services.GenreService;
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
        await genreService.CreateAsync(request);
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<GenreShortResponse>> GetGenre(Guid id)
    {
        var genre = await genreService.GetByIdAsync(id);

        return genre is null ? NotFound($"Genre with id {id} not found") : Ok(genre);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetAll()
    {
        var genres = await genreService.GetAllAsync();

        return Ok(genres);
    }

    [HttpGet("{parentId}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetSubGenres(Guid parentId)
    {
        var genres = await genreService.GetSubGenresAsync(parentId);

        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
    {
        await genreService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
}
