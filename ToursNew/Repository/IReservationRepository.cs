using ToursNew.Models;

namespace ToursNew.Repository
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task DeleteAsync(int id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        Task UpdateAsync(Reservation reservation);
    }
}