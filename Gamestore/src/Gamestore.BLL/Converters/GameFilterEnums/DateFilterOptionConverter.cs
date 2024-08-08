using System.Text.Json;
using System.Text.Json.Serialization;
using Gamestore.BLL.Filtration.Games.Options;

namespace Gamestore.BLL.Converters.GameFilterEnums;

public class DateFilterOptionConverter : JsonConverter<DateFilterOption>
{
    public override DateFilterOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "last week" => DateFilterOption.LastWeek,
            "last month" => DateFilterOption.LastMonth,
            "last year" => DateFilterOption.LastYear,
            "2 years" => DateFilterOption.TwoYears,
            "3 years" => DateFilterOption.ThreeYears,
            _ => throw new JsonException("Unknown value for DateFilterOption"),
        };
    }

    public override void Write(Utf8JsonWriter writer, DateFilterOption value, JsonSerializerOptions options)
    {
        string jsonString = value switch
        {
            DateFilterOption.LastWeek => "last week",
            DateFilterOption.LastMonth => "last month",
            DateFilterOption.LastYear => "last year",
            DateFilterOption.TwoYears => "2 years",
            DateFilterOption.ThreeYears => "3 years",
            _ => throw new InvalidOperationException("Impossible value for DateFilterOption"),
        };
        writer.WriteStringValue(jsonString);
    }
}