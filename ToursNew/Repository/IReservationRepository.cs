using ToursNew.Models;

namespace ToursNew.Repository
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task DeleteAsync(int id);
        IQueryable<Reservation> GetAll();
        Task<Reservation> GetByIdAsync(int id);
        Task UpdateAsync(Reservation reservation);
    }
}