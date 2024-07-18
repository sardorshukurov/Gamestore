using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PublishersController(IPublisherService publisherService, IGameService gameService) : ControllerBase
{
    private readonly IPublisherService _publisherService = publisherService;
    private readonly IGameService _gameService = gameService;

    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherRequest request)
    {
        try
        {
            await _publisherService.CreateAsync(request.AsDto());
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{companyName}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PublisherResponse>> GetByCompanyName(string companyName)
    {
        try
        {
            var publisher = await _publisherService.GetByCompanyNameAsync(companyName);

            return publisher is null
                ? NotFound($"Publisher with company name: {companyName} not found.")
                : Ok(publisher.AsResponse());
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PublisherResponse>>> GetAll()
    {
        try
        {
            var publishers = (await _publisherService.GetAllAsync())
                .Select(p => p.AsResponse());

            return Ok(publishers);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePublisherRequest request)
    {
        try
        {
            await _publisherService.UpdateAsync(request.AsDto());

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
            await _publisherService.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Publisher with id {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }

    [HttpGet("{companyName}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetGamesByCompanyName(string companyName)
    {
        try
        {
            var games = (await _gameService.GetByPublisherAsync(companyName))
                .Select(g => g.AsResponse())
                .ToList();

            return Ok(games);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal server error has occured");
        }
    }
}