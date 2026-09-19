using IMS_DomainLayer.Dtos.Client;
using IMS_DomainLayer.Dtos.Invoice;
using IMS_DomainLayer.Dtos.Users;
using IMS_DomainLayer.Models;

namespace IMS.ViewModels
{
    public class DashboardViewModel
    {
        public int AppUserCount { get; set; }
        public int ClientCount { get; set; }
        public int InvoiceCount { get; set; }

        public double TotalRevenue { get; set; }

        public List<UserDto> Users { get; set; } = new();   
        public List<ClientDto> Clients { get; set; } = new();
        public List<InvoiceDto> Invoices { get; set; } = new();
    }
}
