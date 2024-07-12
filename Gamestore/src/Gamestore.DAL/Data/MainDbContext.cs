using Gamestore.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Data;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Platform> Platforms { get; set; }

    public DbSet<GameGenre> GamesGenres { get; set; }

    public DbSet<GamePlatform> GamesPlatforms { get; set; }
}