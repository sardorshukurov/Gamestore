using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gamestore.DAL.Data.Configuration;

public class OrderGameContextConfiguration : IEntityTypeConfiguration<OrderGame>
{
    public void Configure(EntityTypeBuilder<OrderGame> builder)
    {
        builder
            .HasKey(og => new { og.OrderId, og.ProductId });
        builder
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(og => og.ProductId);
        builder
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(og => og.OrderId);
    }
}