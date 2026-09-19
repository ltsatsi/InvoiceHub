using IMS_InfrastructureLayer.QuestPdf.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IMS_InfrastructureLayer.QuestPdf.Document
{
    public class InvoiceDocument : IDocument
    {
        private InvoiceDocumentModel _model { get; }
        public InvoiceDocument(InvoiceDocumentModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }


        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;


        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);


                    //page footer
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        } // end method


        // header component
        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("INVOICE").FontSize(24).Bold();
                    column.Item().Text($"Invoice No: {_model.InvoiceNumber}");
                });
            });
        } // end method


        // content component
        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(30).Column(column =>
            {
                column.Spacing(5);

                column.Item().Element(ComposeAddress);
                column.Item().Element(ComposeDates);
                column.Item().Element(ComposeItemsTable);
                column.Item().Element(ComposeAmountsTable);

                column.Item()
                    .Text("Note: Late payments will incur a 10% annual fee, calculated daily.")
                    .FontSize(12)
                    .AlignCenter()
                    .FontColor(Color.FromHex("#555555"));
            });
        } // end method

        void ComposeAddress(IContainer container)
        {
            container.PaddingVertical(5).Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Billed By:").SemiBold();
                    column.Item().Text($"{_model.ApplicationUser.FullName}").FontColor(Color.FromHex("#555555"));
                    column.Item().Text($"{_model.ApplicationUser.Email}").FontColor(Color.FromHex("#555555"));
                    column.Item().Text($"{_model.ApplicationUser.AddressLine}").FontColor(Color.FromHex("#555555"));
                });

                row.ConstantItem(50);

                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Billed To:").SemiBold();
                    column.Item().Text($"{_model.Client.FullName}").FontColor(Color.FromHex("#555555"));
                    column.Item().Text($"{_model.Client.Email}").FontColor(Color.FromHex("#555555")); 
                    column.Item().Text($"{_model.Client.AddressLine}").FontColor(Color.FromHex("#555555"));
                });
            });
        }

        void ComposeDates(IContainer container)
        {
            container.PaddingVertical(10).Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Date Issued:").SemiBold();
                    column.Item().Text(_model.DateIssued.ToString("dd MMM yyyy")).FontColor(Color.FromHex("#555555"));
                });

                row.ConstantItem(50);

                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Due Date:").SemiBold();
                    column.Item().Text(_model.DueDate.ToString("dd MMM yyyy")).FontColor(Color.FromHex("#555555"));
                });
            });
        } // end method

        void ComposeAmountsTable(IContainer container)
        {
            container.PaddingVertical(20).Table(table =>
            {

                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.ConstantColumn(120);
                });


                table.Cell().Text("Subtotal");
                table.Cell().AlignRight().Text($"R {_model.Subtotal:N2}");

                table.Cell().Text("Discount");
                table.Cell().AlignRight().Text($"{_model.Discount}%");

                table.Cell().Text("Grand Total").Bold();
                table.Cell().AlignRight().Text($"R {_model.GrandTotal:N2}").Bold();

            });
        } // end method

        void ComposeItemsTable(IContainer container)
        {
            container.Table(table =>
            {

                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });


                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).PaddingBottom(5).Text("Item Name");
                    header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                    header.Cell().Element(CellStyle).AlignRight().Text("Tax");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });


                foreach (var item in _model.InvoiceItems)
                {
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Tax}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Amount}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        } // end method
    }
}
