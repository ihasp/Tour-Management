using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Repository;

namespace ToursNew.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripRepository _tripRepository;

        public TripsController(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var trips = await _tripRepository.GetAllAsync();
            return View(trips);
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
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
                await _tripRepository.AddAsync(trip);
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

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }
            return View(trip);
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
                    await _tripRepository.UpdateAsync(trip);
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

            var trip = await _tripRepository.GetByIdAsync(id.Value);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tripRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TripExists(int id)
        {
            return await _tripRepository.GetByIdAsync(id) != null;
        }
    }
}
