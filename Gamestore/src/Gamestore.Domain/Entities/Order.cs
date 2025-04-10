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
}