namespace Gamestore.Domain.Entities;

public class Ban
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; }

    public BanDuration Duration { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime? EndDate { get; set; }
}