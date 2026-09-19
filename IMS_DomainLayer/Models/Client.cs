using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Models
{
    public class Client : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AddressLine { get; set; }


        public Guid ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
