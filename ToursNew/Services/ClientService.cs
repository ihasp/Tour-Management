using Microsoft.EntityFrameworkCore;
using ToursNew.Models;
using ToursNew.Repository;

namespace ToursNew.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    //entity
    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _clientRepository.GetAll().ToListAsync();
    }

    public async Task<Client> GetClientsByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task AddClientsAsync(Client client)
    {
        await _clientRepository.AddAsync(client);
    }

    public async Task UpdateClientsAsync(Client client)
    {
        await _clientRepository.UpdateAsync(client);
    }

    public async Task DeleteClientsAsync(int id)
    {
        await _clientRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Client>> SearchClientsAsync(string searchString)
    {
        var clients = await _clientRepository.GetAll().ToListAsync();
        return clients.Where(s => s.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                                  s.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<IEnumerable<Client>> SortClientsAsync(string pickSortOrder)
    {
        var clients = await _clientRepository.GetAll().ToListAsync();

        switch (pickSortOrder)
        {
            case "lastname_ascending":
                return clients.OrderBy(s => s.LastName);
            case "lastname_descending":
                return clients.OrderByDescending(s => s.LastName);
            default:
                return clients.OrderBy(s => s.LastName);
        }
    }
}