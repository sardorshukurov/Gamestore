using Gamestore.Domain.Entities;
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
            .HasOne(og => og.Order)
            .WithMany()
            .HasForeignKey(og => og.OrderId);
        builder
            .HasOne(og => og.Product)
            .WithMany(g => g.OrderGames)
            .HasForeignKey(og => og.ProductId);
    }
}