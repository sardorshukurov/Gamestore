using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class Shipper : BaseMongoEntity
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; }

    public string Phone { get; set; }
}