using Gamestore.Domain.Entities.Comments;
using Gamestore.Domain.Entities.Games;
using Gamestore.Domain.Entities.Orders;
using Gamestore.Domain.Entities.Users;
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

    public DbSet<Comment> Comments { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<Ban> Bans { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}