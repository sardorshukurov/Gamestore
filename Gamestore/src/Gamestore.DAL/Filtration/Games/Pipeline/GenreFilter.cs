using Gamestore.DAL.Data;
using Gamestore.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class GenreFilter(MainDbContext dbContext) : IFilter
{
    public async Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.Genres is not null && criteria.Genres.Any())
        {
            var enumerable = games.ToList();
            var gameIds = enumerable.Select(g => g.Id);

            var gameGenres =
                await dbContext.GamesGenres.Where(
                    gg => gameIds.Contains(gg.GameId)
                          && criteria.Genres.Contains(gg.GenreId))
                    .ToListAsync();

            var filteredGameIds = gameGenres.Select(gg => gg.GameId).Distinct();
            games = enumerable.Where(game => filteredGameIds.Contains(game.Id));
        }

        return games;
    }
}