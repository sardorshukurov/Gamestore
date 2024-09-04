using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.Domain.Common;

public abstract class BaseMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}