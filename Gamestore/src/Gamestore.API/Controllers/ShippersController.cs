using Gamestore.BLL.Services.ShipperService;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ShippersController(
    IShipperService shipperService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShippers()
    {
        var shippers = await shipperService.GetShippersAsync();
        return Ok(shippers);
    }
}