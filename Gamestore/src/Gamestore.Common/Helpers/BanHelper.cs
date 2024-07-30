using Gamestore.Common.Exceptions;
using Gamestore.Domain.Entities;

namespace Gamestore.Common.Helpers;

public static class BanHelper
{
    public static readonly Dictionary<BanDuration, string> BanDurations =
    new()
    {
        { BanDuration.OneHour, "1 hour" },
        { BanDuration.OneDay, "1 day" },
        { BanDuration.OneWeek, "1 week" },
        { BanDuration.OneMonth, "1 month" },
        { BanDuration.Permanent, "permanent" },
    };

    private static readonly Dictionary<string, BanDuration> StringToDurationMap =
        BanDurations.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static BanDuration MapStringToDuration(string duration)
    {
        return StringToDurationMap.TryGetValue(duration, out BanDuration result)
            ? result
            : throw new InvalidBanDurationException();
    }
}