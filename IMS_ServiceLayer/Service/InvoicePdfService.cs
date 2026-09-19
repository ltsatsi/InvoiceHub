using IMS_DomainLayer.Dtos.Invoice;
using IMS_InfrastructureLayer.QuestPdf.Document;
using IMS_InfrastructureLayer.QuestPdf.Models;
using IMS_ServiceLayer.IService;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_ServiceLayer.Service
{
    public class InvoicePdfService : IInvoicePdfService
    {
        public byte[] GenerateInvoicePdf(InvoiceDocumentModel invoiceModel)
        {
            var document = new InvoiceDocument(invoiceModel);
            return document.GeneratePdf();
        } // end method
    }
}
