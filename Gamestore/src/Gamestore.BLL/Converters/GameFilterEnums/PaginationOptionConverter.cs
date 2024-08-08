using System.Text.Json;
using System.Text.Json.Serialization;
using Gamestore.BLL.Filtration.Games.Options;

namespace Gamestore.BLL.Converters.GameFilterEnums;

public class PaginationOptionConverter : JsonConverter<PaginationOption>
{
    public override PaginationOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "10" => PaginationOption.Ten,
            "20" => PaginationOption.Twenty,
            "50" => PaginationOption.Fifty,
            "100" => PaginationOption.OneHundred,
            "all" => PaginationOption.All,
            _ => throw new JsonException("Unknown value for PaginationOption"),
        };
    }

    public override void Write(Utf8JsonWriter writer, PaginationOption value, JsonSerializerOptions options)
    {
        string stringValue = value switch
        {
            PaginationOption.Ten => "10",
            PaginationOption.Twenty => "20",
            PaginationOption.Fifty => "50",
            PaginationOption.OneHundred => "100",
            PaginationOption.All => "all",
            _ => throw new InvalidOperationException("Invalid value for PaginationOption"),
        };
        writer.WriteStringValue(stringValue);
    }
}