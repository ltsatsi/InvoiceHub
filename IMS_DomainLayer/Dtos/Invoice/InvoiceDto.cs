using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Dtos.Invoice
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }



        [Display(Name = "Invoice No")]
        public string InvoiceNumber { get; set; }



        [DataType(DataType.DateTime)]
        [Display(Name = "Date Issued")]
        public DateTimeOffset DateIssued { get; set; } = DateTimeOffset.UtcNow;



        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        public DateTimeOffset DueDate { get; set; } = DateTimeOffset.UtcNow;



        public double Subtotal { get; set; }
        public double Discount { get; set; } = 0.0;
        public double GrandTotal { get; set; }


        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}
