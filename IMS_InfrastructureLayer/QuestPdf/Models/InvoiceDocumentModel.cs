using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_InfrastructureLayer.QuestPdf.Models
{   
    public class InvoiceDocumentModel
    {
        public string InvoiceNumber { get; set; }
        public DateTimeOffset DateIssued { get; set; }
        public DateTimeOffset DueDate { get; set; }


        public double Subtotal { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }


        public Client? Client { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }   
        public List<InvoiceItem> InvoiceItems { get; set; } = new();    
    }
}
