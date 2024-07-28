using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.PublisherService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PublishersController(
    IPublisherService publisherService) : ControllerBase
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
        await publisherService.UpdateAsync(request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await publisherService.DeleteAsync(id);

        return NoContent();
    }
}