using Gamestore.BLL.Filtration.Games.Options;
using Gamestore.Domain.Entities;

namespace Gamestore.BLL.Filtration.Games.Pipeline;

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

    private static DateTime GetDateFromFilter(DateFilterOption dateFilterOption)
    {
        return dateFilterOption switch
        {
            DateFilterOption.LastWeek => DateTime.Now.AddDays(-7),
            DateFilterOption.LastMonth => DateTime.Now.AddMonths(-1),
            DateFilterOption.LastYear => DateTime.Now.AddYears(-1),
            DateFilterOption.TwoYears => DateTime.Now.AddYears(-2),
            DateFilterOption.ThreeYears => DateTime.Now.AddYears(-3),
            _ => throw new ArgumentOutOfRangeException(nameof(dateFilterOption), dateFilterOption, null),
        };
    }
}