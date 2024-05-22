using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToursNew.Models;
using ToursNew.Services;
using ToursNew.ViewModels;

namespace ToursNew.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IMapper _mapper;
        private readonly IValidator<Trip> _validator;

        public TripsController(ITripService tripService, IMapper mapper, IValidator<Trip> validator)
        {
            _tripService = tripService;
            _mapper = mapper;
            _validator = validator;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var trips = await _tripService.GetAllTripsAsync();
            var tripViewModels = _mapper.Map<IEnumerable<TripViewModel>>(trips);
            return View(tripViewModels);
        }

        // GET: Search
        public async Task<IActionResult> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _tripService.SearchTripASync(searchString);
            var tripViewModels = _mapper.Map<IEnumerable<TripViewModel>>(searchResults);
            return View("Index", tripViewModels);
        }

        // GET: Sort
        public async Task<IActionResult> Sort(string sortOrder)
        {
            var sortedResults = await _tripService.SortTripsAsync(sortOrder);
            var tripViewModels = _mapper.Map<IEnumerable<TripViewModel>>(sortedResults);
            return View("Index", tripViewModels);
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _tripService.GetTripByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }

            var tripViewModel = _mapper.Map<TripViewModel>(trip);
            return View(tripViewModel);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDTrip,Destination,FromWhere,DepartureDate,ReturnDate,Price,Description")] TripViewModel tripViewModel)
        {
            if (ModelState.IsValid)
            {
                Trip trip = _mapper.Map<Trip>(tripViewModel);
                var validationResult = await _validator.ValidateAsync(trip);
                if (validationResult.IsValid)
                {
                    await _tripService.AddTripAsync(trip);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(tripViewModel);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _tripService.GetTripByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }

            var tripViewModel = _mapper.Map<TripViewModel>(trip);
            return View(tripViewModel);
        }

        // POST: Trips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDTrip,Destination,FromWhere,DepartureDate,ReturnDate,Price,Description")] TripViewModel tripViewModel)
        {
            if (id != tripViewModel.IDTrip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                Trip trip = _mapper.Map<Trip>(tripViewModel);
                var validationResult = await _validator.ValidateAsync(trip);

                if (validationResult.IsValid)
                {
                    try
                    {
                        await _tripService.UpdateTripAsync(trip);
                    }
                    catch
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }

                }

            }
            return View(tripViewModel);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _tripService.GetTripByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }

            var tripViewModel = _mapper.Map<TripViewModel>(trip);
            return View(tripViewModel);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tripService.DeleteTripAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TripExists(int id)
        {
            return await _tripService.GetTripByIdAsync(id) != null;
        }
    }
}
