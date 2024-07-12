using Gamestore.DAL.Data;
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
}