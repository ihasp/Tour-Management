namespace ToursNew.Services;

public interface IActivityLogger
{
    Task LogAsync(string action, string userId, string details = null);
}