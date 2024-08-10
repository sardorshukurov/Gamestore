using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class Order : BaseMongoEntity
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int EmployeeId { get; set; }

    public double Freight { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }

    public string ShipAddress { get; set; }

    public string ShipCity { get; set; }

    public string ShipCountry { get; set; }

    public string ShipName { get; set; }

    public int ShipPostalCode { get; set; }

    public string ShipRegion { get; set; }

    public int ShipVia { get; set; }

    public DateTime ShippedVia { get; set; }
}