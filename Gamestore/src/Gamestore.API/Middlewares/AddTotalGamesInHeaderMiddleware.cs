using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Games;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Gamestore.API.Middlewares;

public class AddTotalGamesInHeaderMiddleware(
    IServiceScopeFactory serviceScopeFactory,
    IMemoryCache cache, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Here we're creating a scope to be able to resolve scoped dependencies:
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var gameRepository = scope.ServiceProvider.GetRequiredService<IRepository<Game>>();
            var optionsSnapshot =
                scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TotalGamesMiddlewareConfig>>();

            var cacheEntryOptions = TimeSpan.FromMinutes(optionsSnapshot.Value.CacheExpirationMinutes);

            var gamesCount = await cache.GetOrCreateAsync("TotalGamesCount", async entry =>
            {
                entry.SetAbsoluteExpiration(cacheEntryOptions);
                return await gameRepository.CountAsync();
            });

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("x-total-number-of-games", gamesCount.ToString());
                return Task.CompletedTask;
            });
        }

        await next(context);
    }
}