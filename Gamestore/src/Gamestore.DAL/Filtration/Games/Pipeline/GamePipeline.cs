using Gamestore.Domain.Entities.Games;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class GamePipeline(List<IFilter> filters)
{
    public async Task<IEnumerable<Game>> ProcessAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        foreach (var filter in filters)
        {
            games = await filter.ApplyAsync(games, criteria);
        }

        return games;
    }
}