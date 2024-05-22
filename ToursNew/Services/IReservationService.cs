using ToursNew.Models;

namespace ToursNew.Services
{
    public interface IReservationService
    {
        Task AddReservationsAsync(Reservation reservation);
        Task DeleteReservationsAsync(int id);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationsByIdAsync(int id);
        Task <IEnumerable<Reservation>> SortReservationsAsync(string sortOrder);
        Task UpdateReservationsAsync(Reservation reservation);
    }
}