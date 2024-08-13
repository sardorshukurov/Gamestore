using Gamestore.Domain.Entities;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class PriceFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        games = games.Where(game => game.Price >= criteria.MinPrice
                                    && game.Price <= criteria.MaxPrice);

        return Task.FromResult(games);
    }
}