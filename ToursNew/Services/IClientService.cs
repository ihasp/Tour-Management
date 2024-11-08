using ToursNew.Models;

namespace ToursNew.Services;

public interface IClientService
{
    Task AddClientsAsync(Client client);
    Task DeleteClientsAsync(int id);
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task<Client> GetClientsByIdAsync(int id);
    Task<IEnumerable<Client>> SearchClientsAsync(string searchString);
    Task<IEnumerable<Client>> SortClientsAsync(string pickSortOrder);
    Task UpdateClientsAsync(Client client);
}