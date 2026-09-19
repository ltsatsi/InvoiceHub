using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Models
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTimeOffset DateIssued { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public double Subtotal { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }


        public Guid ClientId { get; set; }
        public Client? Client { get; set; }  
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}
