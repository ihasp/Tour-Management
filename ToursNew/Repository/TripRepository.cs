using Microsoft.EntityFrameworkCore;
using ToursNew.Data;
using ToursNew.Models;

namespace ToursNew.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly ToursContext _context;
        public TripRepository(ToursContext context)
        {
            _context = context;
        }

        public IQueryable<Trip> GetAll()
        {
            IQueryable<Trip> tripquery = _context.Trips;
            return tripquery;
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task AddAsync(Trip trip)
        {
            _context.Add(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trip trip)
        {
            _context.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
        }
    }
}
