using Gamestore.BLL.DTOs.Game;
using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PublishersController(
    IPublisherService publisherService,
    IGameService gameService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherRequest request)
    {
        await publisherService.CreateAsync(request);
        return Ok();
    }

    [HttpGet("{companyName}")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PublisherResponse>> GetByCompanyName(string companyName)
    {
        var publisher = await publisherService.GetByCompanyNameAsync(companyName);

        return publisher is null
            ? NotFound($"Publisher with company name: {companyName} not found.")
            : Ok(publisher);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<PublisherResponse>>> GetAll()
    {
        var publishers = await publisherService.GetAllAsync();

        return Ok(publishers);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePublisherRequest request)
    {
        try
        {
            await publisherService.UpdateAsync(request);

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
            await publisherService.DeleteAsync(id);

            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound($"Publisher with id {id} not found");
        }
    }

    [HttpGet("{companyName}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetGamesByCompanyName(string companyName)
    {
        try
        {
            var games = await gameService.GetByPublisherAsync(companyName);

            return Ok(games);
        }
        catch (NotFoundException nex)
        {
            return NotFound(nex.Message);
        }
    }
}