using Gamestore.DAL.Data.Configuration;
using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Platform> Platforms { get; set; }

    public DbSet<Publisher> Publishers { get; set; }

    public DbSet<GameGenre> GamesGenres { get; set; }

    public DbSet<GamePlatform> GamesPlatforms { get; set; }

    public DbSet<OrderGame> OrdersGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureGames();
        modelBuilder.ConfigureGenres();
        modelBuilder.ConfigureGameGenres();
        modelBuilder.ConfigureGamePlatforms();
        modelBuilder.ConfigurePlatforms();
        modelBuilder.ConfigurePublishers();
        modelBuilder.ConfigureOrders();
        modelBuilder.ConfigureOrderGame();

        base.OnModelCreating(modelBuilder);
    }
}