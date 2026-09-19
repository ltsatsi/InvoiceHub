using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Dtos.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }    
        public string FullName { get; set; }
        public string AddressLine { get; set; }
        public string Email { get; set; }
    }
}
