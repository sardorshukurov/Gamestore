using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

public class PlatformFilter(IRepository<GamePlatform> gamePlatformRepository) : IFilter
{
    public async Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.Platforms is not null && criteria.Platforms.Any())
        {
            var enumerable = games.ToList();
            var gameIds = enumerable.Select(g => g.Id);

            var gamePlatforms =
                await gamePlatformRepository.GetAllByFilterAsync(
                    gp => gameIds.Contains(gp.GameId)
                          && criteria.Platforms.Contains(gp.PlatformId));
            var filteredGameIds = gamePlatforms.Select(gp => gp.GameId).Distinct();
            games = enumerable.Where(game => filteredGameIds.Contains(game.Id));
        }

        return games;
    }
}