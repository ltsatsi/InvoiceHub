using IMS_DomainLayer.Dtos.Invoice;
using IMS_DomainLayer.Mappers;
using IMS_DomainLayer.Models;
using IMS_InfrastructureLayer.QuestPdf.Models;
using IMS_ServiceLayer.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly ICustomService<Invoice> _invoiceService;
        private readonly ICustomService<Client> _clientService;
        private readonly IInvoicePdfService _invoicePdfService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvoiceController(
            ICustomService<Invoice> invoiceService,
            ICustomService<Client> clientService,
            UserManager<ApplicationUser> userManager,
            IInvoicePdfService invoicePdfService)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));   
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _invoicePdfService = invoicePdfService ?? throw new ArgumentNullException(nameof(invoicePdfService));
        }



        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var invoices = await _invoiceService.GetAllAsync();
            var invoicesDto = invoices.Where(x => x.Client != null && x.Client.ApplicationUserId == user.Id).Select(i => i.ToInvoiceDto()).ToList();


            return View(invoicesDto);
        }




        public async Task<IActionResult> Details(Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            var invoiceDto = invoice.ToInvoiceDto();

            return View(invoiceDto);
        }




        [HttpGet]
        public IActionResult Create(Guid clientId)
        {
            var invoiceDto = new InvoiceDto();
            invoiceDto.ClientId = clientId;
            invoiceDto.InvoiceNumber = InvoiceNumberGenerator();
            return View(invoiceDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return View(invoiceDto);

            var invoice = invoiceDto.ToInvoice();

            var (dtoSubtotal, dtoGrandTotal) = GetAmount(invoiceDto.InvoiceItems.ToList(), invoiceDto.Discount);

            invoice.Subtotal = dtoSubtotal;
            invoice.GrandTotal = dtoGrandTotal;
            invoice.CreatedAt = DateTimeOffset.UtcNow;

            await _invoiceService.CreateAsync(invoice);

            return RedirectToAction("Details", "Client", new { id = invoiceDto.ClientId });
        }




        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            var invoiceDto = invoice.ToInvoiceDto();

            return View(invoiceDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(InvoiceDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return View(invoiceDto);

            var invoice = await _invoiceService.GetByIdAsync(invoiceDto.Id);
            var (dtoSubtotal, dtoGrandTotal) = GetAmount(invoiceDto.InvoiceItems.ToList(), invoiceDto.Discount);

            invoice.DateIssued = invoiceDto.DateIssued;
            invoice.DueDate = invoiceDto.DueDate;
            invoice.Subtotal = dtoSubtotal;
            invoice.Discount = invoiceDto.Discount;
            invoice.GrandTotal = dtoGrandTotal;
            invoice.InvoiceItems = invoiceDto.InvoiceItems;
            invoice.ModifiedOn = DateTimeOffset.UtcNow;

            await _invoiceService.UpdateAsync(invoice);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            return View(invoice.ToInvoiceDto());
        }


        [HttpPost]
        public async Task<IActionResult> Delete(InvoiceDto invoiceDto)
        {
            var invoice = await _invoiceService.GetByIdAsync(invoiceDto.Id);
            await _invoiceService.DeleteAsync(invoice);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> DownloadPdf(Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice is null)
                return NotFound();

            var client = await _clientService.GetByIdAsync(invoice.ClientId);

            if (client is null)
                return NotFound();

            var applicationUser = await _userManager.FindByIdAsync(client.ApplicationUserId.ToString());

            if (applicationUser is null)
                return NotFound();

            InvoiceDocumentModel invoiceDocumentModel = new InvoiceDocumentModel
            {
                InvoiceNumber = invoice.InvoiceNumber,

                DateIssued = invoice.DateIssued,
                DueDate = invoice.DueDate,

                Subtotal = invoice.Subtotal,
                Discount = invoice.Discount,
                GrandTotal = invoice.GrandTotal,

                Client = client,
                ApplicationUser = applicationUser,
                InvoiceItems = invoice.InvoiceItems.ToList()
            };

            var invoicePdf = _invoicePdfService.GenerateInvoicePdf(invoiceDocumentModel);

            return File(invoicePdf, "application/pdf", $"{invoice.InvoiceNumber}.pdf");
        }


        // invoice number generator
        private string InvoiceNumberGenerator()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string nums = "1234567890";

            StringBuilder buildString = new StringBuilder();
            buildString.Append("INV-");

            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                buildString.Append(chars[random.Next(chars.Length)]);
            }
            buildString.Append("-");

            for (int i = 0; i < 3; i++)
            {
                buildString.Append(nums[random.Next(nums.Length)]);
            }

            return buildString.ToString();
        }


        // helper method to calculate subtotal and grandtotal from invoice items
        private (double subtotal, double grandTotal) GetAmount(List<InvoiceItem> items, double discount = 0)
        {
            double subtotal = 0;

            foreach (InvoiceItem item in items)
            {
                double lineTotal = item.Amount * item.Quantity;
                double taxAmount = lineTotal * item.Tax / 100;

                subtotal += lineTotal + taxAmount;
            }

            double grandTotal = subtotal;

            if (discount > 0)
            {
                grandTotal -= subtotal * discount;
            }

            return (subtotal, grandTotal);
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
