using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Repository;

namespace ToursNew.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _reservationRepository.GetAll().ToListAsync();
        }

        public async Task<Reservation> GetReservationsByIdAsync(int id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        public async Task AddReservationsAsync(Reservation reservation)
        {
            await _reservationRepository.AddAsync(reservation);
        }

        public async Task UpdateReservationsAsync(Reservation reservation)
        {
            await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task DeleteReservationsAsync(int id)
        {
            await _reservationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Reservation>> SortReservationsAsync(string sortOrder)
        {
            var reservations = await _reservationRepository.GetAll().ToListAsync();

            switch (sortOrder)
            {
                case "Cash":
                    return reservations.Where(r => r.paymentMethod == PaymentMethod.Pieniędzmi);
                case "Paypal":
                    return reservations.Where(r => r.paymentMethod == PaymentMethod.Paypal);
                case "Transfer":
                    return reservations.Where(r => r.paymentMethod == PaymentMethod.Przelew);
                case "CreditCard":
                    return reservations.Where(r => r.paymentMethod == PaymentMethod.Karta);
                default:
                    return reservations.OrderBy(r => r.ReservationDate);
            }
        }
    }
}
