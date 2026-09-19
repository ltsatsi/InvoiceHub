using IMS.ViewModels;
using IMS_DomainLayer.Mappers;
using IMS_DomainLayer.Models;
using IMS_ServiceLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICustomService<Client> _clientService;
        private readonly ICustomService<Invoice> _invoiceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(ICustomService<Client> clientService, ICustomService<Invoice> invoiceService, UserManager<ApplicationUser> userManager)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }



        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();
            var invoices = await _invoiceService.GetAllAsync();
            var users = _userManager.Users;

            var dashboardViewModel = new DashboardViewModel
            {
                AppUserCount = users.Count(),
                ClientCount = clients.Count(),
                InvoiceCount = invoices.Count(),

                TotalRevenue = invoices.Sum(x => x.GrandTotal),

                Users = users.Select(u => u.ToUserDto()).ToList(),
                Clients = clients.Select(c => c.ToClientDto()).ToList(),
                Invoices = invoices.Select(i => i.ToInvoiceDto()).ToList()
            };  

            return View(dashboardViewModel);
        }
    }
}
