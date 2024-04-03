using ToursNew.Models;

namespace ToursNew.Repository
{
    public interface ITripRepository
    {
        Task AddAsync(Trip trip);
        Task DeleteAsync(int id);
        Task<IEnumerable<Trip>> GetAllAsync();
        Task<Trip> GetByIdAsync(int id);
        Task UpdateAsync(Trip trip);
    }
}