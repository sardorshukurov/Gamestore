using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class Product : BaseMongoEntity
{
    public int CategoryId { get; set; }

    public int Discountinued { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string QuantityPerUnit { get; set; }

    public int ReorderLevel { get; set; }

    public int SupplierId { get; set; }

    public double UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public int UnitsOnOrder { get; set; }
}