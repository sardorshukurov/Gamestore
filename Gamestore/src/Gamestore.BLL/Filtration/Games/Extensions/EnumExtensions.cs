namespace Gamestore.BLL.Filtration.Games.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<string> GetPaginationDisplayValues()
    {
        foreach (var value in Enum.GetValues(typeof(PaginationOption)))
        {
            yield return (int)value == 0 ? "all" : ((int)value).ToString();
        }
    }

    public static string GetFriendlyName(this SortingOption option)
    {
        return option switch
        {
            SortingOption.MostPopular => "Most popular",
            SortingOption.MostCommented => "Most commented",
            SortingOption.PriceAsc => "Price ASC",
            SortingOption.PriceDesc => "Price DESC",
            SortingOption.Newest => "New",
            _ => throw new ArgumentException("Unexpected SortingOption value."),
        };
    }

    public static string GetFriendlyName(this DateFilterOption option)
    {
        return option switch
        {
            DateFilterOption.LastWeek => "last week",
            DateFilterOption.LastMonth => "last month",
            DateFilterOption.LastYear => "last year",
            DateFilterOption.TwoYears => "2 years",
            DateFilterOption.ThreeYears => "3 years",
            _ => throw new ArgumentException("Unexpected DateFilterOption value."),
        };
    }
}