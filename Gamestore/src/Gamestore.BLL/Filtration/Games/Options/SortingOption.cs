using System.Text.Json.Serialization;
using Gamestore.BLL.Converters.GameFilterEnums;

namespace Gamestore.BLL.Filtration.Games.Options;

[JsonConverter(typeof(SortingOptionConverter))]
public enum SortingOption
{
    MostPopular,
    MostCommented,
    PriceAsc,
    PriceDesc,
    Newest,
}