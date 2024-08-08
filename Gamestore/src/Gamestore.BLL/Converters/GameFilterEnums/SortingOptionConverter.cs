using System.Text.Json;
using System.Text.Json.Serialization;
using Gamestore.BLL.Filtration.Games.Options;

namespace Gamestore.BLL.Converters.GameFilterEnums;

public class SortingOptionConverter : JsonConverter<SortingOption>
{
    public override SortingOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "Most popular" => SortingOption.MostPopular,
            "Most commented" => SortingOption.MostCommented,
            "Price ASC" => SortingOption.PriceAsc,
            "Price DESC" => SortingOption.PriceDesc,
            "New" => SortingOption.Newest,
            _ => throw new JsonException("Unknown value for SortingOption"),
        };
    }

    public override void Write(Utf8JsonWriter writer, SortingOption value, JsonSerializerOptions options)
    {
        string stringValue = value switch
        {
            SortingOption.MostPopular => "Most popular",
            SortingOption.MostCommented => "Most commented",
            SortingOption.PriceAsc => "Price ASC",
            SortingOption.PriceDesc => "Price DESC",
            SortingOption.Newest => "New",
            _ => throw new InvalidOperationException("Invalid value for SortingOption"),
        };
        writer.WriteStringValue(stringValue);
    }
}