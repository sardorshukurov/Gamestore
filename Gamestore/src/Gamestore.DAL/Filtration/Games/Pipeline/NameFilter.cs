using Gamestore.Domain.Entities.Games;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class NameFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        var filteredGames = criteria.Name is not null ?
            games.Where(g => string.Equals(g.Name, criteria.Name, StringComparison.OrdinalIgnoreCase))
            : games;

        return Task.FromResult(filteredGames);
    }
}