using Gamestore.API.DTOs.Game;
using Gamestore.API.DTOs.Publisher;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PublishersController(
    IPublisherService publisherService,
    IGameService gameService,
    CreatePublisherValidator createValidator,
    UpdatePublisherValidator updateValidator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherRequest request)
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

            await publisherService.CreateAsync(request.AsDto());
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
            var publisher = await publisherService.GetByCompanyNameAsync(companyName);

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
            var publishers = (await publisherService.GetAllAsync())
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
            var result = await updateValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                });
            }

            await publisherService.UpdateAsync(request.AsDto());

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
            await publisherService.DeleteAsync(id);

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
            var games = (await gameService.GetByPublisherAsync(companyName))
                .Select(g => g.AsResponse())
                .ToList();

            return Ok(games);
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
}