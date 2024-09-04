using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class Territory : BaseMongoEntity
{
    public int TerritoryId { get; set; }

    public string TerritoryDescription { get; set; }

    public int RegionId { get; set; }
}