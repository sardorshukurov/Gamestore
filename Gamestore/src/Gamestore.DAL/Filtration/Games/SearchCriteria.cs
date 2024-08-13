using Gamestore.DAL.Filtration.Games.Options;

namespace Gamestore.DAL.Filtration.Games;

public class SearchCriteria
{
    public List<Guid>? Genres { get; set; }

    public List<Guid>? Platforms { get; set; }

    public List<Guid>? Publishers { get; set; }

    public double MinPrice { get; set; }

    public double MaxPrice { get; set; } = double.MaxValue;

    public string? Name { get; set; }

    public DateFilterOptions? PublishingDate { get; set; }

    public SortingOptions SortBy { get; set; } = SortingOptions.MostPopular;

    public int Page { get; set; } = 1;

    public PaginationOptions PageCount { get; set; } = PaginationOptions.All;
}