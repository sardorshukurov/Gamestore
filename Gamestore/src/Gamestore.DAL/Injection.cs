using Gamestore.DAL.Data;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;
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
        services.AddScoped<IRepository<Genre>, Repository<Genre>>();
        services.AddScoped<IRepository<Platform>, Repository<Platform>>();
        services.AddScoped<IRepository<GameGenre>, Repository<GameGenre>>();
        services.AddScoped<IRepository<GamePlatform>, Repository<GamePlatform>>();

        return services;
    }
}