using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities;

public class Game : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string Key { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public string QuantityPerUnit { get; set; }

    public int ReorderLevel { get; set; }

    public int UnitInStock { get; set; }

    public int UnitOnOrder { get; set; }

    public int Discount { get; set; }

    public DateTime PublishingDate { get; set; }

    public Guid PublisherId { get; set; }

    public virtual ICollection<GameGenre> GameGenres { get; set; }

    public virtual ICollection<GamePlatform> GamePlatforms { get; set; }

    public virtual ICollection<OrderGame> OrderGames { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public int? OriginalId { get; set; }
}