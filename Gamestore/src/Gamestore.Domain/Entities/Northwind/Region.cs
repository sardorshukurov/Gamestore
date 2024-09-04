using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class Region : BaseMongoEntity
{
    public int RegionId { get; set; }

    public string RegionDescription { get; set; }
}