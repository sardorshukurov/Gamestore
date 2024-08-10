namespace Gamestore.BLL.Services.ShipperService;

public interface IShipperService
{
    Task<IEnumerable<dynamic>> GetShippersAsync();
}