using IMS_DomainLayer.Models;
using IMS_RepositoryLayer.IRepository;
using IMS_ServiceLayer.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_ServiceLayer.Service
{
    public class ClientService : ICustomService<Client>
    {
        private readonly IRepository<Client> _clientRepo;
        public ClientService(IRepository<Client> clientRepo)
        {
            _clientRepo = clientRepo ?? throw new ArgumentNullException(nameof(clientRepo));
        }


        public async Task CreateAsync(Client entity)
        {
            try
            {
                await _clientRepo.CreateAsync(entity);
                await _clientRepo.SaveChangesAsync();   
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Client entity)
        {
            try
            {
                await _clientRepo.DeleteAsync(entity);
                await _clientRepo.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            try
            {
                var clients =  await _clientRepo.GetAllAsync();
                var invoicesIncluded = await clients.Include(c => c.Invoices).ToListAsync();

                return invoicesIncluded;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            try
            {
                //return await _clientRepo.GetByIdAsync(id);
                var clients = await _clientRepo.GetAllAsync();
                return await clients.Include(c => c.Invoices).FirstOrDefaultAsync(c => c.Id == id);
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            try
            {
                return await _clientRepo.GetByIdAsync(id) != null;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Client entity)
        {
            try
            {
                await _clientRepo.UpdateAsync(entity);
                await _clientRepo.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
