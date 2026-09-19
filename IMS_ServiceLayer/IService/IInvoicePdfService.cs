using IMS_DomainLayer.Dtos.Invoice;
using IMS_InfrastructureLayer.QuestPdf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_ServiceLayer.IService
{
    public interface IInvoicePdfService
    {
        byte[] GenerateInvoicePdf(InvoiceDocumentModel invoiceModel);
    }
}
