using System.Text.Json.Serialization;
using Gamestore.BLL.Converters.GameFilterEnums;

namespace Gamestore.BLL.Filtration.Games.Options;

[JsonConverter(typeof(DateFilterOptionConverter))]
public enum DateFilterOption
{
    LastWeek,
    LastMonth,
    LastYear,
    TwoYears,
    ThreeYears,
}