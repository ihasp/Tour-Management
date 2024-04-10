using Microsoft.EntityFrameworkCore;
using ToursNew.Data;
using ToursNew.Models;
using ToursNew.Controllers;

namespace ToursNew.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ToursContext _context;

        public ClientRepository(ToursContext context)
        {

            _context = context;

        }


        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task AddAsync(Client client)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();

            }
        }

    }
}
