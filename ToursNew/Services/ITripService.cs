using ToursNew.Models;

namespace ToursNew.Services
{
    public interface ITripService
    {
        Task AddTripAsync(Trip trip);
        Task DeleteTripAsync(int id);
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> GetTripByIdAsync(int id);
        Task<IEnumerable<Trip>> SearchTripASync(string searchString);
        Task<IEnumerable<Trip>> SortTripsAsync(string sortOrder);
        Task UpdateTripAsync(Trip trip);
    }
}