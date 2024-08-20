using Gamestore.Domain.Entities;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class PublisherFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.Publishers is not null && criteria.Publishers.Any())
        {
            games = games.Where(game => criteria.Publishers.Contains(game.PublisherId));
        }

        return Task.FromResult(games);
    }
}