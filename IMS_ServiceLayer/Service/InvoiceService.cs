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
    public class InvoiceService : ICustomService<Invoice>
    {
        private readonly IRepository<Invoice> _invoiceRepo;
        public InvoiceService(IRepository<Invoice> invoiceRepo)
        {
            _invoiceRepo = invoiceRepo ?? throw new ArgumentNullException(nameof(invoiceRepo));
        }


        public async Task CreateAsync(Invoice entity)
        {
            try
            {
                await _invoiceRepo.CreateAsync(entity);
                await _invoiceRepo.SaveChangesAsync();  
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Invoice entity)
        {
            try
            {
                await _invoiceRepo.DeleteAsync(entity);
                await _invoiceRepo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            try
            {
                var invoices = await _invoiceRepo.GetAllAsync();
                var invoiceItemsIncluded = await invoices.Include(i => i.Client).Include(i => i.InvoiceItems).ToListAsync();

                return invoiceItemsIncluded;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<Invoice> GetByIdAsync(Guid id)
        {
            try
            {
                //return await _invoiceRepo.GetByIdAsync(id);
                var invoices = await _invoiceRepo.GetAllAsync();
                return await invoices.Include(i => i.InvoiceItems).FirstOrDefaultAsync(i => i.Id == id);
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            try
            {
                return await _invoiceRepo.GetByIdAsync(id) != null;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Invoice entity)
        {
            try
            {
                await _invoiceRepo.UpdateAsync(entity);
                await _invoiceRepo.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
