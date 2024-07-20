using Gamestore.DAL.Common;

namespace Gamestore.DAL.Entities;

public enum OrderStatus
{
    Open,
    Checkout,
    Paid,
    Cancelled,
}

public class Order : IBaseEntity
{
    public Guid Id { get; set; }

    public DateTime? Date { get; set; }

    public Guid CustomerId { get; set; }

    public OrderStatus Status { get; set; }
}