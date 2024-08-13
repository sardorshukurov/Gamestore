using Gamestore.DAL.Filtration.Games.Options;
using Gamestore.Domain.Entities;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class PaginationFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        var filteredGames = criteria.PageCount == PaginationOptions.All ? games
            : games.Skip((criteria.Page - 1) * (int)criteria.PageCount).Take((int)criteria.PageCount);

        return Task.FromResult(filteredGames);
    }
}