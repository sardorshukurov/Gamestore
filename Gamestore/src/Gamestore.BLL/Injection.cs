using Gamestore.BLL.Services.CommentService;
using Gamestore.BLL.Services.GameService;
using Gamestore.BLL.Services.GenreService;
using Gamestore.BLL.Services.OrderService;
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
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}