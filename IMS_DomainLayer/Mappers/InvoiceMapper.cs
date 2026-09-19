using IMS_DomainLayer.Dtos.Invoice;
using IMS_DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Mappers
{
    public static class InvoiceMapper
    {
        public static Invoice ToInvoice(this InvoiceDto invoiceDto)
        {
            return new Invoice
            {
                ClientId = invoiceDto.ClientId,

                InvoiceNumber = invoiceDto.InvoiceNumber,
                DateIssued = invoiceDto.DateIssued,
                DueDate = invoiceDto.DueDate,
                Subtotal = invoiceDto.Subtotal,
                Discount = invoiceDto.Discount,
                GrandTotal = invoiceDto.GrandTotal,
                InvoiceItems = invoiceDto.InvoiceItems,
                IsActive = true,
            };
        }

        public static InvoiceDto ToInvoiceDto(this Invoice invoice)
        {
            return new InvoiceDto
            {
                Id = invoice.Id,
                ClientId = invoice.ClientId,

                InvoiceNumber = invoice.InvoiceNumber,
                DateIssued = invoice.DateIssued,
                DueDate = invoice.DueDate,
                Subtotal = invoice.Subtotal,
                Discount = invoice.Discount,
                GrandTotal = invoice.GrandTotal,
                InvoiceItems = invoice.InvoiceItems,
            };
        }
    }
}
