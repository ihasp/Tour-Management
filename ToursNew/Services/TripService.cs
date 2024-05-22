using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Repository;

namespace ToursNew.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            return await _tripRepository.GetAll().ToListAsync();
        }

        public async Task<Trip> GetTripByIdAsync(int id)
        {
            return await _tripRepository.GetByIdAsync(id);
        }

        public async Task AddTripAsync(Trip trip)
        {
            await _tripRepository.AddAsync(trip);
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            await _tripRepository.UpdateAsync(trip);
        }

        public async Task DeleteTripAsync(int id)
        {
            await _tripRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Trip>> SearchTripASync(string searchString)
        {
            var trips = await _tripRepository.GetAll().ToListAsync();
            return trips.Where(t => t.Destination.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) 
                                    || t.Description.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));                 
        }

        public async Task<IEnumerable<Trip>> SortTripsAsync(string sortOrder)
        {
            var trips = await _tripRepository.GetAll().ToListAsync();

            switch (sortOrder)
            {
                case "price_asc":
                    return trips.OrderBy(t => t.Price);
                case "price_desc":
                    return trips.OrderByDescending(t => t.Price);
                case "destination_asc":
                    return trips.OrderBy(t => t.Destination);
                case "destination_desc":
                    return trips.OrderByDescending(t => t.Destination);
                case "departure_asc":
                    return trips.OrderBy(t => t.DepartureDate);
                case "departure_desc":
                    return trips.OrderByDescending(t => t.DepartureDate);
                case "return_asc":
                    return trips.OrderBy(t=>t.ReturnDate);
                case "return_desc":
                    return trips.OrderByDescending(t => t.ReturnDate);
                default:
                    return trips.OrderBy(t => t.DepartureDate);
            }
        }
    }
}
