﻿using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToursNew.Areas.Identity.Pages.Account.Manage;
using ToursNew.Data;
using ToursNew.Models;
using ToursNew.Services;
using ToursNew.ViewModels;

namespace ToursNew.Controllers;

[Authorize(Roles = "User, Admin, Manager")]
public class ClientsController : Controller
{
    private readonly IClientService _clientService;
    private readonly ToursContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<Client> _validator;

    public ClientsController(IClientService clientService, IMapper mapper, IValidator<Client> validator,
        ToursContext context)
    {
        _clientService = clientService;
        _mapper = mapper;
        _validator = validator;
        _context = context;
    }

    public string LicenseState { get; private set; }

    // GET: Clients
    public async Task<IActionResult> Index()
    {
        LicenseState = LicenseModel.GetLicenseState();
        var clients = await _clientService.GetAllClientsAsync();
        var clientViewModels = _mapper.Map<IEnumerable<ClientViewModel>>(clients);
        return View(clientViewModels);
    }

    // GET: Clients/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var client = await _clientService.GetClientsByIdAsync(id.Value);
        if (client == null) return NotFound();

        var clientViewModel = _mapper.Map<ClientViewModel>(client);
        return View(clientViewModel);
    }

    // GET: Clients/Search
    public async Task<IActionResult> Search(string searchString)
    {
        if (string.IsNullOrEmpty(searchString)) return RedirectToAction(nameof(Index));

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
    [Authorize(Roles = "Admin, Manager")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Clients/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("IDClient,Name,LastName,Email,Phone,Adult")]
        ClientViewModel clientViewModel)
    {
        if (ModelState.IsValid)
        {
            var client = _mapper.Map<Client>(clientViewModel);
            var validationResult = await _validator.ValidateAsync(client);
            if (validationResult.IsValid)
            {
                await _clientService.AddClientsAsync(client);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in validationResult.Errors) ModelState.AddModelError(string.Empty, error.ErrorMessage);
        }

        return View(clientViewModel);
    }

    // GET: Clients/Edit/5
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var client = await _clientService.GetClientsByIdAsync(id.Value);
        if (client == null) return NotFound();

        var clientViewModel = _mapper.Map<ClientViewModel>(client);
        return View(clientViewModel);
    }

    // POST: Clients/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("IDClient,Name,LastName,Email,Phone,Adult")]
        ClientViewModel clientViewModel)
    {
        if (id != clientViewModel.IDClient) return NotFound();

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
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(clientViewModel);
    }

    // GET: Clients/Delete/5
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var client = await _clientService.GetClientsByIdAsync(id.Value);
        if (client == null) return NotFound();

        var clientViewModel = _mapper.Map<ClientViewModel>(client);
        return View(clientViewModel);
    }

    // POST: Clients/Delete/5
    [HttpPost]
    [ActionName("Delete")]
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

    //export to csv
    [HttpGet]
    public IActionResult ExportToFile()
    {
        var clients = _context.Clients
            .Select(c => new ClientViewModel
            {
                Name = c.Name,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Adult = c.Adult
            }).ToList();

        var csv = new StringBuilder();
        csv.AppendLine("Name,LastName,Email,Phone,Adult");

        foreach (var client in clients)
            csv.AppendLine($"{client.Name},{client.LastName},{client.Email},{client.Phone},{client.Adult}");

        var fileName = "Clients.csv";
        var fileBytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(fileBytes, "text/csv", fileName);
    }
}