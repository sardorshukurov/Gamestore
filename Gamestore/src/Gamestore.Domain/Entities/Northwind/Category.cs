using Gamestore.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.Domain.Entities.Northwind;

public class Category : BaseMongoEntity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    [BsonRepresentation(BsonType.Binary)]
    public byte[] Picture { get; set; }
}