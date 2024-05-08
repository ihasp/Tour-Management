using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Services;
using ToursNew.ViewModels;

//TODO: implement fluentvalidation, like in clients controller (edit, create)
namespace ToursNew.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IMapper _mapper;

        public TripsController(ITripService tripService, IMapper mapper)
        {
            _tripService = tripService;
            _mapper = mapper;
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
        public async Task<IActionResult> Create([Bind("IDTrip,Destination,FromWhere,DepartureDate,ReturnDate,Price,Description")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                await _tripService.AddTripAsync(trip);
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
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
        public async Task<IActionResult> Edit(int id, [Bind("IDTrip,Destination,FromWhere,DepartureDate,ReturnDate,Price,Description")] Trip trip)
        {
            if (id != trip.IDTrip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tripService.UpdateTripAsync(trip);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TripExists(trip.IDTrip))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trip);
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
