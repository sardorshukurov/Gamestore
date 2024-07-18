using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class GameContextConfiguration
{
    public static void ConfigureGames(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(g => g.Id);
        modelBuilder.Entity<Game>()
            .Property(g => g.Name)
            .IsRequired();
        modelBuilder.Entity<Game>()
            .HasIndex(g => g.Key)
            .IsUnique();
        modelBuilder.Entity<Game>()
            .Property(g => g.Description)
            .IsRequired(false);
        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .IsRequired();
        modelBuilder.Entity<Game>()
            .Property(g => g.UnitInStock)
            .IsRequired();
        modelBuilder.Entity<Game>()
            .Property(g => g.Discount)
            .IsRequired();
        modelBuilder.Entity<Game>()
            .Property(g => g.PublisherId)
            .IsRequired();
        modelBuilder.Entity<Game>()
            .HasOne<Publisher>()
            .WithMany()
            .HasForeignKey(g => g.PublisherId);
    }
}