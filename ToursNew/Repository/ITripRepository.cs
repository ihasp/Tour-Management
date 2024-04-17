using ToursNew.Models;

namespace ToursNew.Repository
{
    public interface ITripRepository
    {
        Task AddAsync(Trip trip);
        Task DeleteAsync(int id);
        IQueryable<Trip> GetAll();
        Task<Trip> GetByIdAsync(int id);
        Task UpdateAsync(Trip trip);
    }
}