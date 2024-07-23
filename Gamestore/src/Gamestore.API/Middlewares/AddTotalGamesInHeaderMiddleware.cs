using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Gamestore.API.Middlewares;

public class AddTotalGamesInHeaderMiddleware(IServiceScopeFactory serviceScopeFactory, IMemoryCache cache, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var cacheEntryOptions = TimeSpan.FromMinutes(1);

        // Here we're creating a scope to be able to resolve scoped dependencies:
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var gameRepository = scope.ServiceProvider.GetRequiredService<IRepository<Game>>();

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