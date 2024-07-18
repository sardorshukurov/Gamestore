using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class PublisherContextConfiguration
{
    public static void ConfigurePublishers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Publisher>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Publisher>()
            .Property(p => p.CompanyName)
            .IsRequired();
        modelBuilder.Entity<Publisher>()
            .HasIndex(p => p.CompanyName)
            .IsUnique();
        modelBuilder.Entity<Publisher>()
            .Property(p => p.HomePage)
            .IsRequired(false);
        modelBuilder.Entity<Publisher>()
            .Property(p => p.Description)
            .IsRequired(false);
    }
}