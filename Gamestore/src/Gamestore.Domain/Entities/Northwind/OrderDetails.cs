using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class OrderDetails : BaseMongoEntity
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double UnitPrice { get; set; }

    public float Discount { get; set; }
}