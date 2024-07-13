using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class PlatformContextConfiguration
{
    public static void ConfigurePlatforms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Platform>()
            .Property(p => p.Type)
            .IsRequired();
        modelBuilder.Entity<Platform>()
            .HasIndex(p => p.Type)
            .IsUnique();
    }
}