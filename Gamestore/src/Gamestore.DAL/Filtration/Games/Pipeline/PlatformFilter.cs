using Gamestore.DAL.Data;
using Gamestore.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class PlatformFilter(MainDbContext dbContext) : IFilter
{
    public async Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.Platforms is not null && criteria.Platforms.Any())
        {
            var enumerable = games.ToList();
            var gameIds = enumerable.Select(g => g.Id);

            var gamePlatforms =
                await dbContext.GamesPlatforms.Where(
                        gp => gameIds.Contains(gp.GameId)
                              && criteria.Platforms.Contains(gp.PlatformId))
                    .ToListAsync();

            var filteredGameIds = gamePlatforms.Select(gp => gp.GameId).Distinct();
            games = enumerable.Where(game => filteredGameIds.Contains(game.Id));
        }

        return games;
    }
}