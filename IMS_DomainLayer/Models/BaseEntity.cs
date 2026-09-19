using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }  
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}
