using Gamestore.BLL.DTOs.Genre;
using Gamestore.BLL.Services.GenreService;
using Gamestore.DAL.Data.Seeder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController(
    IGenreService genreService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
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

    [HttpGet("{gameKey}/game")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GenreShortResponse>>> GetGenresByKey(string gameKey)
    {
        var genres = await genreService.GetAllByGameKeyAsync(gameKey);

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
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
    public async Task<IActionResult> Update(UpdateGenreRequest request)
    {
        await genreService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{UserRolesHolder.Administrator}, {UserRolesHolder.Manager}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await genreService.DeleteAsync(id);
        return NoContent();
    }
}
