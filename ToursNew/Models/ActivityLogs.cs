namespace ToursNew.Models;

public class ActivityLogs
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}