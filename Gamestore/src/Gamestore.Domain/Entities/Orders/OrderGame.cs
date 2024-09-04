using Gamestore.Domain.Entities.Games;

namespace Gamestore.Domain.Entities.Orders;

public class OrderGame
{
    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; }

    public Guid ProductId { get; set; }

    public virtual Game Product { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public float Discount { get; set; }
}