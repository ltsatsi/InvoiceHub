using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string AddressLine { get; set; }


        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
