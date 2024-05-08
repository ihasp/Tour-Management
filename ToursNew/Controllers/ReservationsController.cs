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
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            var reservationViewModels = _mapper.Map<IEnumerable<ReservationViewModel>>(reservations);
            return View("Index", reservationViewModels);
        }

        // Get: Sort
        public async Task<IActionResult> Sort(string sortOrder)
        {
            var sortedReservations = await _reservationService.SortReservationsAsync(sortOrder);
            var reservationViewModels = _mapper.Map<IEnumerable<ReservationViewModel>>(sortedReservations);
            return View("Index", reservationViewModels);
        }


        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationsByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            var reservationViewModel = _mapper.Map<ReservationViewModel>(reservation);
            return View(reservationViewModel);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDReservation,IDClient,IDTrip,ReservationDate,paymentMethod,paymentStatus")] ReservationViewModel reservationViewModel)
        {
            if (ModelState.IsValid)
            {
                var reservation = _mapper.Map<Reservation>(reservationViewModel);
                await _reservationService.AddReservationsAsync(reservation);
                return RedirectToAction(nameof(Index));
            }
            return View(reservationViewModel);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationsByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            var reservationViewModel = _mapper.Map<ReservationViewModel>(reservation);
            return View(reservationViewModel);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDReservation,IDClient,IDTrip,ReservationDate,paymentMethod,paymentStatus")] ReservationViewModel reservationViewModel)
        {
            if (id != reservationViewModel.IDReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var reservation = _mapper.Map<Reservation>(reservationViewModel);
                try
                {
                    await _reservationService.UpdateReservationsAsync(reservation);
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
            return View(reservationViewModel);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationsByIdAsync(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            var reservationViewModel = _mapper.Map<ReservationViewModel>(reservation);
            return View(reservationViewModel);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationService.DeleteReservationsAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ReservationExists(int id)
        {
            return await _reservationService.GetReservationsByIdAsync(id) != null;
        }
    }
}