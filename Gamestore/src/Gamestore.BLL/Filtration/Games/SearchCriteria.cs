using Gamestore.BLL.Filtration.Games.Options;

namespace Gamestore.BLL.Filtration.Games;

public class SearchCriteria
{
    public List<Guid>? Genres { get; set; }

    public List<Guid>? Platforms { get; set; }

    public List<Guid>? Publishers { get; set; }

    public double MinPrice { get; set; }

    public double MaxPrice { get; set; } = double.MaxValue;

    public string? Name { get; set; }

    public DateFilterOption? PublishingDate { get; set; }

    public SortingOption SortBy { get; set; }

    public int Page { get; set; } = 1;

    public PaginationOption PageCount { get; set; }
}