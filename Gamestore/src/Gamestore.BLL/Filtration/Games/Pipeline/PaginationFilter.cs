using Gamestore.BLL.Filtration.Games.Options;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

public class PaginationFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        var filteredGames = criteria.PageCount == PaginationOption.All ? games
            : games.Skip((criteria.Page - 1) * (int)criteria.PageCount).Take((int)criteria.PageCount);

        return Task.FromResult(filteredGames);
    }
}