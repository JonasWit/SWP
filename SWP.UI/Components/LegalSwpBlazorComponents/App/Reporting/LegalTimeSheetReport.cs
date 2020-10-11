using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App.Reporting
{
    [UITransientService]
    public class LegalTimeSheetReport : PdfReportFooterStandard
    {
        private PdfWriter _pdfWriter;
        private int _maxColumn = 8;
        private Document _document;
        private PdfPTable _pdfPTable = new PdfPTable(8);
        private PdfPCell _pdfPCell;
        private MemoryStream _memoryStream = new MemoryStream();
        private ReportData _reportData;
        private readonly IWebHostEnvironment _env;
        private readonly IJSRuntime _jSRuntime;

        public class ReportData
        {
            public string NIP { get; set; }
            public List<TimeRecordViewModel> Records { get; set; }
            public string ClientName { get; set; }
            public string ReportName { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool UseSelectedMonth { get; set; }
        }

        public string FontPath => Path.Combine(_env.WebRootPath, "Fonts");

        public LegalTimeSheetReport(IWebHostEnvironment env, IJSRuntime jSRuntime)
        {
            _env = env;
            _jSRuntime = jSRuntime;
        }
            
        public void GeneratePDF(ReportData reportData)
        {
            _reportData = reportData;
            _jSRuntime.InvokeAsync<List<TimeRecordViewModel>>(
                    "saveAsFile",
                    $"{_reportData.ReportName}.pdf",
                    Convert.ToBase64String(Report())
                );
        }

        public byte[] Report()
        {
            _document = new Document(PageSize.A4, 10f, 10f, 20f, 30f);
            _pdfPTable.WidthPercentage = 100;
            _pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            FontFactory.Register(FontPath);

            _pdfWriter = PdfWriter.GetInstance(_document, _memoryStream);
            _pdfWriter.PageEvent = new PdfReportFooterStandard();

            _document.Open();

            float[] sizes = new float[_maxColumn];

            for (int i = 0; i < _maxColumn; i++)
            {
                if (i == 0)
                {
                    sizes[i] = 50;
                }
                else
                {
                    sizes[i] = 100;
                }
            }

            _pdfPTable.SetWidths(sizes);

            ReportHeader();
            ReportBody();

            _pdfPTable.HeaderRows = 2;
            _document.Add(_pdfPTable);

            this.OnEndPage(_pdfWriter, _document);

            _document.Close();

            return _memoryStream.ToArray();
        }

        private void ReportBody()
        {
            var headersFontStyle = FontFactory.GetFont("Anonymous_Pro", 10f, 0);
            var dataFontStyle = FontFactory.GetFont("Anonymous_Pro", 8f, 0);

            _pdfPCell = new PdfPCell(new Phrase($"NIP : ", headersFontStyle))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_reportData.NIP, headersFontStyle))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase($"Nazwa Klienta : ", headersFontStyle))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_reportData.ClientName, headersFontStyle))
            {
                Colspan = 4,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("Data Faktury : ", headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_reportData.InvoiceDate.ToString("dd.MM.yyyy"), headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("Numer Faktury : ", headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase(_reportData.InvoiceNumber, headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("Przedział czasowy : ", headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase($"{_reportData.StartDate:dd.MM.yyyy} - {_reportData.EndDate:dd.MM.yyyy}", headersFontStyle))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0,
                ExtraParagraphSpace = 0
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            #region Table Header

            _pdfPCell = new PdfPCell(new Phrase("Nr.", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Data (Date)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Prawnik (Lawyer)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Nazwa (Name)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Opis (Description)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Godziny (Hours)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Stawka (Rate)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Suma (Total)", headersFontStyle))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = BaseColor.White
            };
            _pdfPTable.AddCell(_pdfPCell);

            _pdfPTable.CompleteRow();

            #endregion

            #region TimeSheet Table Body

            int rowNumber = 1;

            foreach (var record in _reportData.Records)
            {
                _pdfPCell = new PdfPCell(new Phrase(rowNumber++.ToString(), dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.EventDate.ToString("dd.MM.yyyy"), dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.Lawyer, dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.Description, dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.TimeSpent, dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.Rate.FormatPLN(), dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(record.Total.FormatPLN(), dataFontStyle))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray
                };
                _pdfPTable.AddCell(_pdfPCell);

                _pdfPTable.CompleteRow();
            }

            #endregion
        }

        private void ReportHeader()
        {
            var fontStyle = FontFactory.GetFont("Anonymous_Pro", 18f, 1);

            _pdfPCell = new PdfPCell(new Phrase("Wykaz czynności objętych fakturą (Invoice Details)", fontStyle))
            {
                Colspan = _maxColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
                ExtraParagraphSpace = 0
            };

            _pdfPTable.AddCell(_pdfPCell);
            _pdfPTable.CompleteRow();
        }






    }
}
