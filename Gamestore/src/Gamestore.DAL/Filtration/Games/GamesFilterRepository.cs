using Gamestore.DAL.Data;
using Gamestore.DAL.Filtration.Games.Pipeline;
using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DAL.Filtration.Games;

public class GamesFilterRepository(
    MainDbContext dbContext) : IGamesFilterRepository
{
    public async Task<ICollection<Game>> GetAsync(SearchCriteria criteria)
    {
        var allGames = await dbContext.Games
            .Include(g => g.GameGenres)
            .Include(g => g.GamePlatforms)
            .Include(g => g.OrderGames)
            .Include(g => g.Comments)
            .ToListAsync();

        List<IFilter> filters =
        [
            new GenreFilter(dbContext),
            new PublisherFilter(),
            new PlatformFilter(dbContext),
            new NameFilter(),
            new PriceFilter(),
            new DateFilter(),
            new SortingFilter(),
            new PaginationFilter()
        ];
        var pipeline = new GamePipeline(filters);

        return (await pipeline.ProcessAsync(allGames, criteria))
            .ToList();
    }
}