using ToursNew.Data;
using ToursNew.Models;
namespace ToursNew.Services;



public class ActivityLogger : IActivityLogger
{
    private readonly ToursContext _context;

    public ActivityLogger(ToursContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string action, string userId, string details = null)
    {
        var log = new ActivityLogs
        {
            Action = action,
            UserId = userId,
            Timestamp = DateTime.UtcNow,
            Details = details
        };

        _context.ActivityLogs.Add(log);
        await _context.SaveChangesAsync();
    }

}