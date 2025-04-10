namespace Gamestore.Domain.Entities;

public class OrderGame
{
    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; }

    public Guid ProductId { get; set; }

    public virtual Game Product { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public int Discount { get; set; }
}