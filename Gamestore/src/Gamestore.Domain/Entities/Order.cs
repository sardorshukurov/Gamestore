using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public enum OrderStatus
{
    Open,
    Checkout,
    Paid,
    Cancelled,
}

public class Order : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime? Date { get; set; }

    public Guid CustomerId { get; set; }

    public OrderStatus Status { get; set; }

    public int EmployeeId { get; set; }

    public double Freight { get; set; }

    public string ShipAddress { get; set; }

    public string ShipCity { get; set; }

    public string ShipCountry { get; set; }

    public string ShipName { get; set; }

    public int ShipPostalCode { get; set; }

    public string ShipRegion { get; set; }

    public int ShipVia { get; set; }

    public DateTime ShippedVia { get; set; }

    public DateTime RequiredDate { get; set; }
}