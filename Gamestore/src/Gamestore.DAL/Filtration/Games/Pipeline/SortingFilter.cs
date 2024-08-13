using Gamestore.DAL.Filtration.Games.Options;
using Gamestore.Domain.Entities;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class SortingFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        return Task.FromResult<IEnumerable<Game>>(criteria.SortBy switch
        {
            SortingOptions.MostPopular => games.OrderByDescending(game => game.OrderGames.Count),
            SortingOptions.MostCommented => games.OrderByDescending(game => game.Comments.Count),
            SortingOptions.PriceAsc => games.OrderBy(game => game.Price),
            SortingOptions.PriceDesc => games.OrderByDescending(game => game.Price),
            SortingOptions.Newest => games.OrderByDescending(game => game.PublishingDate),
            _ => throw new ArgumentException("Invalid sorting option provided."),
        });
    }
}