using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class GamePlatformContextConfiguration
{
    public static void ConfigureGamePlatforms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GamePlatform>()
            .HasKey(gp => new { gp.GameId, gp.PlatformId });
        modelBuilder.Entity<GamePlatform>()
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(gp => gp.GameId);
        modelBuilder.Entity<GamePlatform>()
            .HasOne<Platform>()
            .WithMany()
            .HasForeignKey(gp => gp.PlatformId);
    }
}