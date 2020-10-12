using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWP.UI.Components.LegalSwpBlazorComponents.App.Reporting;
using Org.BouncyCastle.Asn1.X509;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ProductivityPage : BlazorPageBase
    {
        private CreateTimeRecord CreateTimeRecord => serviceProvider.GetService<CreateTimeRecord>();
        private DeleteTimeRecord DeleteTimeRecord => serviceProvider.GetService<DeleteTimeRecord>();
        private UpdateTimeRecord UpdateTimeRecord => serviceProvider.GetService<UpdateTimeRecord>();
        private LegalTimeSheetReport LegalTimeSheetReport => serviceProvider.GetService<LegalTimeSheetReport>();

        public LegalBlazorApp App { get; private set; }
        public CreateTimeRecord.Request NewTimeRecord { get; set; } = new CreateTimeRecord.Request();
        public LegalTimeSheetReport.ReportData NewTimesheetReport { get; set; } = new LegalTimeSheetReport.ReportData();

        public ProductivityPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            GetDataForMonthFilter();
            return Task.CompletedTask;
        }

        public RadzenGrid<TimeRecordViewModel> TimeRecordsGrid { get; set; }

        public void EditTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.EditRow(time);

        public async Task OnUpdateTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                var result = await UpdateTimeRecord.Update(new UpdateTimeRecord.Request
                {
                    Id = time.Id,
                    Rate = time.Rate,
                    Lawyer = time.Lawyer,
                    Description = time.Description,
                    Name = time.Name,
                    EventDate = time.EventDate,
                    RecordedHours = time.RecordedHours,
                    RecordedMinutes = time.RecordedMinutes,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                if (App.ActiveClient != null)
                {
                    App.ActiveClientWithData.TimeRecords[App.ActiveClientWithData.TimeRecords.FindIndex(x => x.Id == result.Id)] = result;
                }

                await TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.UpdateRow(time);

        public void CancelTimeRecordEdit(TimeRecordViewModel time)
        {
            TimeRecordsGrid.CancelEditRow(time);
            App.RefreshClientWithData();
        }

        public async Task DeleteTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                await DeleteTimeRecord.Delete(time.Id);
                App.ActiveClientWithData.TimeRecords.RemoveAll(x => x.Id == time.Id);
                App.ActiveClientWithData.SelectedTimeRecord = null;

                await TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Wpis: {time.Name}, został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewTimeRecord(CreateTimeRecord.Request arg)
        {
            if (arg.RecordedTime == new TimeSpan(0, 0, 0))
            {
                return;
            }

            try
            {
                NewTimeRecord.UpdatedBy = App.User.UserName;

                var result = await CreateTimeRecord.Create(App.ActiveClient.Id, App.User.Profile, NewTimeRecord);
                NewTimeRecord = new CreateTimeRecord.Request();

                App.ActiveClientWithData.TimeRecords.Add(result);
                await TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveTimeRecordChange(object value)
        {
            var input = (TimeRecordViewModel)value;
            if (value != null)
            {
                App.ActiveClientWithData.SelectedTimeRecord = App.ActiveClientWithData.TimeRecords.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedTimeRecord = null;
            }
        }

        public void GetDataForMonthFilter()
        {
            if (App.ActiveClientWithData == null) return;

            int id = 1;
            MonthsFilterData.Clear();

            foreach (var record in App.ActiveClientWithData.TimeRecords)
            {
                var year = record.EventDate.Year;
                var month = record.EventDate.Month;

                if (!MonthsFilterData.Any(x => x.Month == month && x.Year == year))
                {
                    MonthsFilterData.Add(new MonthFilterRecord { Id = id, Month = month, Year = year });
                    id++;
                }
            }

            if (MonthsFilterData.Count != 0)
            {
                MonthsFilterData.OrderBy(x => x.Year + x.Month);
            }
        }

        public void SelectedMonthChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                SelectedMonth = MonthsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                SelectedMonth = null;
            }
        }

        public async Task GenerateTimesheetReport(LegalTimeSheetReport.ReportData reportData)
        {
            try
            {
                var productivityRecords = new List<TimeRecordViewModel>();

                if (reportData.UseSelectedMonth)
                {
                    if (SelectedMonth != null)
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
                reportData.ReportName = $"{reportData.ClientName}_{DateTime.Now:yyyy-MM-dd-hh-mm-ss}";
                LegalTimeSheetReport.GeneratePDF(reportData);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }
    }
}
