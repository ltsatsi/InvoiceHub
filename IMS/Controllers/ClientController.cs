using IMS_DomainLayer.Dtos.Client;
using IMS_DomainLayer.Mappers;
using IMS_DomainLayer.Models;
using IMS_ServiceLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMS.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ICustomService<Client> _clientService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ClientController(ICustomService<Client> clientService, UserManager<ApplicationUser> userManager)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var clients = await _clientService.GetAllAsync();
            var clientDtos = clients.Where(c => c.ApplicationUserId == user.Id).Select(c => c.ToClientDto()).ToList();

            return View(clientDtos);
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            var clientDto = client.ToClientDto();

            return View(clientDto);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View(new ClientDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientDto clientDto)
        {
            if (!ModelState.IsValid)
                return View(clientDto);

            var user = await GetCurrentUserAsync();
            var client = clientDto.ToClient();
            client.ApplicationUserId = user.Id;
            client.CreatedAt = DateTime.UtcNow;
            await _clientService.CreateAsync(client);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return View(client.ToClientDto());
        }

        [HttpPost]
        public async Task<IActionResult> Update(ClientDto clientDto)
        {
            if (!ModelState.IsValid) 
                return View(clientDto);

            var user = await GetCurrentUserAsync();
            var client = await _clientService.GetByIdAsync(clientDto.Id);

            client.FullName = clientDto.FullName;
            client.Email = clientDto.Email;
            client.AddressLine = clientDto.AddressLine;
            client.ApplicationUserId = user.Id;
            client.ModifiedOn = DateTime.UtcNow;

            await _clientService.UpdateAsync(client);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return View(client.ToClientDto());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ClientDto clientDto)
        {
            var user = await GetCurrentUserAsync();
            var client = clientDto.ToClient();

            client.Id = clientDto.Id;
            client.ApplicationUserId = user.Id;

            await _clientService.DeleteAsync(client);

            return RedirectToAction(nameof(Index));
        }

        // helper method to get the current logged-in user
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException($"User not authorized: {username}");

            var user = await _userManager.FindByNameAsync(username);

            return user ?? throw new ArgumentNullException($"User not authorized: {user}");
        }
    }
}
