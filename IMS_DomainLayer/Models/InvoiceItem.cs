using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Models
{
    public class InvoiceItem : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Tax { get; set; }
        public double Amount { get; set; }


        public Guid InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }    
    }
}
