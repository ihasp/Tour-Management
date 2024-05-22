using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Services;
using ToursNew.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ToursNew.Controllers
{

    [Authorize(Roles = "Admin, Manager")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IValidator<Client> _validator;

        public ClientsController(IClientService clientService, IMapper mapper, IValidator<Client> validator)
        {
            _clientService = clientService;
            _mapper = mapper;
            _validator = validator;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllClientsAsync();
            var clientViewModels = _mapper.Map<IEnumerable<ClientViewModel>>(clients);
            return View(clientViewModels);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClientsByIdAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return View(clientViewModel);
        }

        // GET: Clients/Search
        public async Task<IActionResult> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction(nameof(Index));
            }

            var searchResult = await _clientService.SearchClientsAsync(searchString);
            var clientViewModels = _mapper.Map<IEnumerable<ClientViewModel>>(searchResult);
            return View("Index", clientViewModels);
        }

        // GET: Clients/Sort
        public async Task<IActionResult> Sort(string sortOrder)
        {
            var sortedResults = await _clientService.SortClientsAsync(sortOrder);
            var clientViewModels = _mapper.Map<IEnumerable<ClientViewModel>>(sortedResults);
            return View("Index", clientViewModels);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDClient,Name,LastName,Email,Phone,Adult")] ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                Client client = _mapper.Map<Client>(clientViewModel);
                var validationResult = await _validator.ValidateAsync(client);
                if (validationResult.IsValid)
                {
                    await _clientService.AddClientsAsync(client);
                    return RedirectToAction(nameof(Index));
                }   
                else
                {
                    foreach(var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(clientViewModel);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClientsByIdAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return View(clientViewModel);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDClient,Name,LastName,Email,Phone,Adult")] ClientViewModel clientViewModel)
        {
            if (id != clientViewModel.IDClient)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = _mapper.Map<Client>(clientViewModel);
                try
                {
                    await _clientService.UpdateClientsAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.IDClient))
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
            return View(clientViewModel);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClientsByIdAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            var clientViewModel = _mapper.Map<ClientViewModel>(client);
            return View(clientViewModel);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientsAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _clientService.GetClientsByIdAsync(id) != null;
        }
    }
}
