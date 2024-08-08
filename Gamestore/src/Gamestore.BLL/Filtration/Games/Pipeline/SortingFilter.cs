using Gamestore.BLL.Filtration.Games.Options;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

public class SortingFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        return Task.FromResult<IEnumerable<Game>>(criteria.SortBy switch
        {
            SortingOption.MostPopular => games.OrderByDescending(game => game.OrderGames.Count),
            SortingOption.MostCommented => games.OrderByDescending(game => game.Comments.Count),
            SortingOption.PriceAsc => games.OrderBy(game => game.Price),
            SortingOption.PriceDesc => games.OrderByDescending(game => game.Price),
            SortingOption.Newest => games.OrderByDescending(game => game.PublishingDate),
            _ => throw new ArgumentException("Invalid sorting option provided."),
        });
    }
}