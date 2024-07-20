using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class OrderGameContextConfiguration
{
    public static void ConfigureOrderGame(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderGame>()
            .HasKey(og => new { og.OrderId, og.ProductId });
        modelBuilder.Entity<OrderGame>()
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(og => og.ProductId);
        modelBuilder.Entity<OrderGame>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(og => og.OrderId);
    }
}