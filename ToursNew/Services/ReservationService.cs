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
            return await _reservationRepository.GetAllAsync();
        }

        public async Task<Reservation> GetReservationsById(int id)
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

        public async Task<IEnumerable<Reservation>> searchReservationASync(string searchString)
        {
            var reservations = await _reservationRepository.GetAllAsync();
            try
            {
                return reservations.Where(r => r.paymentStatus.Equals(searchString));
            }

            catch (Exception ex)
            {
                return reservations.Where(r => r.paymentStatus.Equals("WentThrough"));
            }

        }

        public async Task<IEnumerable<Reservation>> SortReservationsAsync(string sortOrder)
        {
            var reservations = await _reservationRepository.GetAllAsync();

            switch (sortOrder)
            {
                case "Cash":
                    return reservations.OrderBy(r => r.paymentMethod.Equals("Cash"));
                case "Paypal":
                    return reservations.OrderBy(r => r.paymentMethod.Equals("Paypal"));
                case "Transfer":
                    return reservations.OrderBy(r => r.paymentMethod.Equals("Transfer"));
                case "CreditCard":
                    return reservations.OrderBy(r => r.paymentMethod.Equals("CreditCard"));
                default:
                    return reservations.OrderBy(r => r.ReservationDate);

            }
        }


    }
}
