using Microsoft.EntityFrameworkCore;
using ToursNew.Data;
using ToursNew.Models;

namespace ToursNew.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ToursContext _context;

        public ReservationRepository(ToursContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task AddAsync(Reservation reservation)
        {
            _context.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
