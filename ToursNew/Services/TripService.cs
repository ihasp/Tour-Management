using ToursNew.Models;
using ToursNew.Repository;

namespace ToursNew.Services
{
    public class TripService
    {
        private readonly ITripRepository _tripRepository;
        public TripService(ITripRepository tripRepository) 
        {
            _tripRepository = tripRepository;
        }

        
    }
}
