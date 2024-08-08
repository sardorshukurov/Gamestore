using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

public class GenreFilter(IRepository<GameGenre> gameGenreRepository) : IFilter
{
    public async Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.Genres is not null && criteria.Genres.Any())
        {
            var enumerable = games.ToList();
            var gameIds = enumerable.Select(g => g.Id);

            var gameGenres =
                await gameGenreRepository.GetAllByFilterAsync(
                    gg => gameIds.Contains(gg.GameId)
                          && criteria.Genres.Contains(gg.GenreId));

            var filteredGameIds = gameGenres.Select(gg => gg.GameId).Distinct();
            games = enumerable.Where(game => filteredGameIds.Contains(game.Id));
        }

        return games;
    }
}