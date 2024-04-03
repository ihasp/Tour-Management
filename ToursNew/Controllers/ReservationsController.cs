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
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationsController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDReservation,IDClient,IDTrip,ReservationDate,paymentMethod,paymentStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                await _reservationRepository.AddAsync(reservation);
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDReservation,IDClient,IDTrip,ReservationDate,paymentMethod,paymentStatus")] Reservation reservation)
        {
            if (id != reservation.IDReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservationRepository.UpdateAsync(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ReservationExists(reservation.IDReservation))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ReservationExists(int id)
        {
            return await _reservationRepository.GetByIdAsync(id) != null;
        }
    }
}
