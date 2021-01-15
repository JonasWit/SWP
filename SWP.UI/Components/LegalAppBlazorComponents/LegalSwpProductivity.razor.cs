using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.Domain.Enums;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Services.Reporting;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.Productivity;
using SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions;
using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalAppBlazorComponents
{
    public partial class LegalSwpProductivity : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ProductivityStore ProductivityStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }
        [Inject]
        public IServiceProvider ServiceProvider { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            ProductivityStore.RemoveStateChangeListener(RefreshView);
            ProductivityStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void RefreshView()
        {
            if (MainStore.GetState().ActiveClient == null)
            {
                MainStore.SetActivePanel(LegalAppPanels.MyApp);
                return;
            }

            ProductivityStore.CleanUpStore();
            ProductivityStore.RefreshSore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(RefreshView);
            ProductivityStore.AddStateChangeListener(RefreshView);
            ProductivityStore.Initialize();
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

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
                using var scope = ServiceProvider.CreateScope();
                var legalTimeSheetReport = scope.ServiceProvider.GetRequiredService<LegalTimeSheetReport>();

                var productivityRecords = new List<TimeRecordViewModel>();

                if (ProductivityStore.GetState().SelectedFont != null)
                {
                    reportData.FontName = ProductivityStore.GetState().SelectedFont.FontName;
                }
                else
                {
                    reportData.FontName = "Anonymous_Pro";
                }

                if (reportData.UseSelectedMonth)
                {
                    if (ProductivityStore.GetState().SelectedMonth != null)
                    {
                        var month = ProductivityStore.GetState().SelectedMonth.Month;
                        var year = ProductivityStore.GetState().SelectedMonth.Year;

                        productivityRecords = ProductivityStore.GetState().TimeRecords
                            .Where(x => x.EventDate.Month == month && x.EventDate.Year == year).ToList();

                        reportData.StartDate = new DateTime(year, month, 1);
                        reportData.EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    }
                    else
                    {
                        MainStore.ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Najpierw przefiltruj dane po wybranym miesiącu", GeneralViewModel.NotificationDuration);
                        return;
                    }
                }
                else
                {
                    productivityRecords = ProductivityStore.GetState().TimeRecords
                        .Where(x => x.EventDate >= reportData.StartDate && x.EventDate <= reportData.EndDate).ToList();
                }

                if (productivityRecords.Count == 0)
                {
                    MainStore.ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Nie wykryto żadnych wpisów w wybranym przedziale dat", GeneralViewModel.NotificationDuration);
                    return;
                }

                reportData.ClientName = MainStore.GetState().ActiveClient.Name;
                reportData.Records = productivityRecords;
                reportData.ReportName = $"Rozliczenie_{DateTime.Now:yyyy-MM-dd-hh-mm-ss}";
                await legalTimeSheetReport.GeneratePDF(reportData);
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        #region Actions

        private void EditTimeRecordRow(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new EditTimeRecordRowAction { Arg = arg });

        private void AddRecordRowToCashMovements(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new AddRecordRowToCashMovementsAction { Arg = arg });

        private void OnUpdateTimeRecordRow(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new OnUpdateTimeRecordRowAction { Arg = arg });

        private void SaveTimeRecordRow(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new SaveTimeRecordRowAction { Arg = arg });

        private void CancelTimeRecordEdit(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new CancelTimeRecordEditAction { Arg = arg });

        private void DeleteTimeRecordRow(TimeRecordViewModel arg) => ActionDispatcher.Dispatch(new DeleteTimeRecordRowAction { Arg = arg });

        private void SubmitNewTimeRecord(CreateTimeRecord.Request arg) => ActionDispatcher.Dispatch(new SubmitNewTimeRecordAction { Arg = arg });

        private void ActiveTimeRecordChange(object arg) => ActionDispatcher.Dispatch(new ActiveTimeRecordChangeAction { Arg = arg });

        private void SelectedMonthChange(object arg) => ActionDispatcher.Dispatch(new SelectedMonthChangeProductivityAction { Arg = arg });

        private void SelectedFontChange(object arg) => ActionDispatcher.Dispatch(new SelectedFontChangeAction { Arg = arg });
        
        #endregion
    }
}
