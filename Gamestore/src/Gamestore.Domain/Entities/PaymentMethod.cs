using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class PaymentMethod : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ImageUrl { get; set; }

    public string Title { get; set; }

    public string Description { get; set; } = string.Empty;
}