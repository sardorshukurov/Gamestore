using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class EntityChangeLog : BaseMongoEntity
{
    public DateTime Timestamp { get; set; }

    public string Action { get; set; }

    public string EntityType { get; set; }

    public Dictionary<string, string>? OldVersion { get; set; }

    public Dictionary<string, string> NewVersion { get; set; }
}