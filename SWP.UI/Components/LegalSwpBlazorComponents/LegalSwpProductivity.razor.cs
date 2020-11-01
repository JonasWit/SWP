using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.App.Reporting;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpProductivity
    {
        [Inject]
        public LegalBlazorApp App { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public LegalTimeSheetReport LegalTimeSheetReport { get; set; }

        public bool showFirstSection = false;
        public void ShowHideFirstSection() => showFirstSection = !showFirstSection;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;
        
        public bool showThirdSection = false;
        public void ShowHideThirdSection() => showThirdSection = !showThirdSection;

        public bool infoBoxVisibleI = false;
        public void ShowHideInfoBoxI() => infoBoxVisibleI = !infoBoxVisibleI;

        public bool infoBoxVisibleII = false;
        public void ShowHideInfoBoxII() => infoBoxVisibleII = !infoBoxVisibleII;

        public bool infoBoxVisibleIII = false;
        public void ShowHideInfoBoxIII() => infoBoxVisibleIII = !infoBoxVisibleIII;

        public async Task GenerateTimesheetReport(LegalTimeSheetReport.ReportData reportData)
        {
            try
            {
                var legalTimeSheetReport = LegalTimeSheetReport;

                var productivityRecords = new List<TimeRecordViewModel>();

                if (App.ProductivityPage.SelectedFont != null)
                {
                    reportData.FontName = App.ProductivityPage.SelectedFont.FontName;
                }
                else
                {
                    reportData.FontName = "Anonymous_Pro";
                }

                if (reportData.UseSelectedMonth)
                {
                    if (App.ProductivityPage.SelectedMonth != null)
                    {
                        var month = App.ProductivityPage.SelectedMonth.Month;
                        var year = App.ProductivityPage.SelectedMonth.Year;

                        productivityRecords = App.ActiveClientWithData.TimeRecords
                            .Where(x => x.EventDate.Month == month && x.EventDate.Year == year).ToList();

                        reportData.StartDate = new DateTime(year, month, 1);
                        reportData.EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    }
                    else
                    {
                        App.ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Najpierw przefiltruj dane po wybranym miesiącu", GeneralViewModel.NotificationDuration);
                        return;
                    }
                }
                else
                {
                    productivityRecords = App.ActiveClientWithData.TimeRecords
                        .Where(x => x.EventDate >= reportData.StartDate && x.EventDate <= reportData.EndDate).ToList();
                }

                if (productivityRecords.Count == 0)
                {
                    App.ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Nie wykryto żadnych wpisów w wybranym przedziale dat", GeneralViewModel.NotificationDuration);
                    return;
                }

                reportData.ClientName = App.ActiveClient.Name;
                reportData.Records = productivityRecords;
                reportData.ReportName = $"Rozliczenie_{DateTime.Now:yyyy-MM-dd-hh-mm-ss}";
                await legalTimeSheetReport.GeneratePDF(reportData);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }
    }
}
