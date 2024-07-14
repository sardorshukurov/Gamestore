using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Gamestore.API.Middlewares;

public class AddTotalGamesInHeaderMiddleware(IRepository<Game> gameRepository, IMemoryCache cache) : IMiddleware
{
    private readonly IRepository<Game> _gameRepository = gameRepository;
    private readonly IMemoryCache _cache = cache;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

        var gamesCount = await _cache.GetOrCreateAsync("TotalGamesCount", async entry =>
        {
            entry.SetOptions(cacheEntryOptions);
            return await _gameRepository.CountAsync();
        });

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("x-total-number-of-games", gamesCount.ToString());
            return Task.CompletedTask;
        });

        await next(context);
    }
}