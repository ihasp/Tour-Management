using ToursNew.Models;

namespace ToursNew.Services
{
    public interface IReservationService
    {
        Task AddReservationsAsync(Reservation reservation);
        Task DeleteReservationsAsync(int id);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationsById(int id);
        Task<IEnumerable<Reservation>> searchReservationASync(string searchString);
        Task<IEnumerable<Reservation>> SortReservationsAsync(string sortOrder);
        Task UpdateReservationsAsync(Reservation reservation);
    }
}