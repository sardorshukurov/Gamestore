using MongoDB.Bson;
using MongoDB.Driver;

namespace Gamestore.BLL.Services.ShipperService;

public class ShipperService(IMongoDatabase database) : IShipperService
{
    private readonly IMongoCollection<BsonDocument> _shippersCollection = database.GetCollection<BsonDocument>("shippers");

    public async Task<IEnumerable<dynamic>> GetShippersAsync()
    {
        var shippers = await _shippersCollection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();
        return shippers.Select(BsonTypeMapper.MapToDotNetValue);
    }
}