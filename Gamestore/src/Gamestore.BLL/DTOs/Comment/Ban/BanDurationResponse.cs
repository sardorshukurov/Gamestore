namespace Gamestore.BLL.DTOs.Comment.Ban;

public static class BanDurationResponse
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
}