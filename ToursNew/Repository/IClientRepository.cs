using ToursNew.Models;

namespace ToursNew.Repository
{
    public interface IClientRepository
    {
        Task AddAsync(Client client);
        Task DeleteAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> GetByIdAsync(int id);
        Task UpdateAsync(Client client);
    }
}