using Gamestore.Domain.Entities.Northwind;
using MongoDB.Driver;

namespace Gamestore.Common.Helpers;

public static class NorthwindHelper
{
    public static void FillProductGuids(MongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("northwind");
        var products = database.GetCollection<Product>("Products");

        var filter = Builders<Product>.Filter.Empty;

        var allProducts = products.Find(filter).ToList();
        foreach (var product in allProducts)
        {
            var specificUpdate = Builders<Product>.Update.Set(p => p.GameStoreId, Guid.NewGuid());
            products.UpdateOne(p => p.Id == product.Id, specificUpdate);
        }
    }
}