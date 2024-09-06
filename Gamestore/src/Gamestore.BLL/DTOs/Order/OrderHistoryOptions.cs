namespace Gamestore.BLL.DTOs.Order;

public class OrderHistoryOptions
{
    public DateTime? Start { get; set; } = DateTime.MinValue;

    public DateTime? End { get; set; } = DateTime.MaxValue;
}
