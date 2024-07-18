using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.PlatformService;
using Gamestore.BLL.Services.PublisherService;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.BLL;

public static class Injection
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPlatformService, PlatformService>();
        services.AddScoped<IPublisherService, PublisherService>();

        return services;
    }
}