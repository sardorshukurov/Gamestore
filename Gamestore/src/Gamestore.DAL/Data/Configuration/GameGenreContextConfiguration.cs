using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data.Configuration;

public static class GameGenreContextConfiguration
{
    public static void ConfigureGameGenres(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameGenre>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });
        modelBuilder.Entity<GameGenre>()
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(gg => gg.GameId);
        modelBuilder.Entity<GameGenre>()
            .HasOne<Genre>()
            .WithMany()
            .HasForeignKey(gg => gg.GenreId);
    }
}