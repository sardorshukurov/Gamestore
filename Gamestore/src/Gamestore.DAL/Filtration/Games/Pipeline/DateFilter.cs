using Gamestore.DAL.Filtration.Games.Options;
using Gamestore.Domain.Entities.Games;

namespace Gamestore.DAL.Filtration.Games.Pipeline;

public class DateFilter : IFilter
{
    public Task<IEnumerable<Game>> ApplyAsync(IEnumerable<Game> games, SearchCriteria criteria)
    {
        if (criteria.PublishingDate.HasValue)
        {
            var filterDate = GetDateFromFilter(criteria.PublishingDate.Value);
            games = games.Where(game => game.PublishingDate >= filterDate);
        }

        return Task.FromResult(games);
    }

    private static DateTime GetDateFromFilter(DateFilterOptions dateFilterOptions)
    {
        return dateFilterOptions switch
        {
            DateFilterOptions.LastWeek => DateTime.Now.AddDays(-7),
            DateFilterOptions.LastMonth => DateTime.Now.AddMonths(-1),
            DateFilterOptions.LastYear => DateTime.Now.AddYears(-1),
            DateFilterOptions.TwoYears => DateTime.Now.AddYears(-2),
            DateFilterOptions.ThreeYears => DateTime.Now.AddYears(-3),
            _ => throw new ArgumentOutOfRangeException(nameof(dateFilterOptions), dateFilterOptions, null),
        };
    }
}