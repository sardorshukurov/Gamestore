using System.Text.Json.Serialization;
using Gamestore.BLL.Converters.GameFilterEnums;

namespace Gamestore.BLL.Filtration.Games.Options;

[JsonConverter(typeof(PaginationOptionConverter))]
public enum PaginationOption
{
    Ten = 10,
    Twenty = 20,
    Fifty = 50,
    OneHundred = 100,
    All = 0,
}