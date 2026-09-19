using IMS_DomainLayer.Dtos.Invoice;
using System.ComponentModel.DataAnnotations;

namespace IMS_DomainLayer.Dtos.Client
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; } 


        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 20 characters")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Display(Name = "Address Line")]
        [Required(ErrorMessage = "Address Line is required")]
        [StringLength(52, MinimumLength = 3, ErrorMessage = "Address Line can only be between 3 and 20 characters")]
        public string AddressLine { get; set; }


        public ICollection<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
    }
}
