using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App.Reporting
{
    public class PdfReportFooterStandard : PdfPageEventHelper
    {
        private readonly Font pageNumberFont = new Font(Font.NORMAL, 8f, Font.NORMAL, BaseColor.Black);

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            AddPageNumber(writer, document);
        }

        public void AddPageNumber(PdfWriter writer, Document document)
        {
            var numberTable = new PdfPTable(1);
            string text = "Strona : " + writer.PageNumber.ToString("00"),
                text1 = "Wygenerowano : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            var pdfCell = new PdfPCell(new Phrase(text, pageNumberFont))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                BackgroundColor = BaseColor.White
            };
            numberTable.AddCell(pdfCell);

            pdfCell = new PdfPCell(new Phrase(text1, pageNumberFont))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                BackgroundColor = BaseColor.White
            };
            numberTable.AddCell(pdfCell);

            numberTable.TotalWidth = 450;
            numberTable.WriteSelectedRows(0, -1, document.Left + 80, document.Bottom + 10, writer.DirectContent);
        }
    }
}
