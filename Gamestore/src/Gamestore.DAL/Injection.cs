using Gamestore.DAL.Data;
using Gamestore.DAL.Filtration.Games;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.DAL;

public static class Injection
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Game>, Repository<Game>>();
        services.AddScoped<IGamesFilterRepository, GamesFilterRepository>();

        services.AddScoped<IRepository<Genre>, Repository<Genre>>();
        services.AddScoped<IRepository<Platform>, Repository<Platform>>();
        services.AddScoped<IRepository<GameGenre>, Repository<GameGenre>>();
        services.AddScoped<IRepository<GamePlatform>, Repository<GamePlatform>>();

        services.AddScoped<IRepository<Publisher>, Repository<Publisher>>();

        services.AddScoped<IRepository<Order>, Repository<Order>>();
        services.AddScoped<IRepository<OrderGame>, Repository<OrderGame>>();

        services.AddScoped<IRepository<Comment>, Repository<Comment>>();

        return services;
    }
}