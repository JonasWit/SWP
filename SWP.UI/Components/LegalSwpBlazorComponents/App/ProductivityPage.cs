﻿using Radzen;
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
using SWP.Application.LegalSwp.CashMovements;
using SWP.Domain.Models.SWPLegal;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ProductivityPage : BlazorPageBase
    {
        public LegalBlazorApp App { get; private set; }
        public CreateTimeRecord.Request NewTimeRecord { get; set; } = new CreateTimeRecord.Request();
        public LegalTimeSheetReport.ReportData NewTimesheetReport { get; set; } = new LegalTimeSheetReport.ReportData();

        public ProductivityPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public FontFilterRecord SelectedFont { get; set; }
        public List<FontFilterRecord> FontsFilterData { get; set; } = new List<FontFilterRecord>();

        public class FontFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText { get; set; }
            public string FontName { get; set; }
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            GetDataForMonthFilter();
            GetDataForFontFilter();
            return Task.CompletedTask;
        }

        public RadzenGrid<TimeRecordViewModel> TimeRecordsGrid { get; set; }

        public void EditTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.EditRow(time);

        public async Task AddRecordRowToCashMovements(TimeRecordViewModel time)
        {
            if (time.Total == 0)
            {
                App.ShowNotification(NotificationSeverity.Warning, "Błąd!", $"Nie można przenieść do Finansów wejscia, z kwotą 0 zł", GeneralViewModel.NotificationDuration);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createCashMovement = scope.ServiceProvider.GetRequiredService<CreateCashMovement>();

                var request = new CreateCashMovement.Request
                {
                    Amount = time.Total,
                    CashFlowDirection = 1,
                    EventDate = time.EventDate,
                    Expense = false,
                    Name = time.Name,
                    UpdatedBy = App.User.UserName
                };

                var result = await createCashMovement.Create(App.ActiveClient.Id, App.User.Profile, request);

                App.ActiveClientWithData.CashMovements.Add(result);

                if (App.FinancePage.CashMovementGrid != null)
                {
                    await App.FinancePage.CashMovementGrid.Reload();
                }

                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana do Panelu Finanse", GeneralViewModel.NotificationDuration);
                App.SetActivePanel(LegalBlazorApp.Panels.Finance);
                App.ForceRefresh();
                App.FinancePage.CashMovementGrid.LastPage();
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public async Task OnUpdateTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateTimeRecord = scope.ServiceProvider.GetRequiredService<UpdateTimeRecord>();

                var result = await updateTimeRecord.Update(new UpdateTimeRecord.Request
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
                await App.ErrorPage.DisplayMessageAsync(ex);
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
                using var scope = _serviceProvider.CreateScope();
                var deleteTimeRecord = scope.ServiceProvider.GetRequiredService<DeleteTimeRecord>();

                await deleteTimeRecord.Delete(time.Id);
                App.ActiveClientWithData.TimeRecords.RemoveAll(x => x.Id == time.Id);
                App.ActiveClientWithData.SelectedTimeRecord = null;

                await TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Wpis: {time.Name}, został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public async Task SubmitNewTimeRecord(CreateTimeRecord.Request request)
        {
            if (request.RecordedTime == new TimeSpan(0, 0, 0)) return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createTimeRecord = scope.ServiceProvider.GetRequiredService<CreateTimeRecord>();

                request.UpdatedBy = App.User.UserName;

                var result = await createTimeRecord.Create(App.ActiveClient.Id, App.User.Profile, request);
                NewTimeRecord = new CreateTimeRecord.Request();

                App.ActiveClientWithData.TimeRecords.Add(result);
                await TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
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

        public void GetDataForFontFilter()
        {
            FontsFilterData.Clear();

            FontsFilterData.Add(new FontFilterRecord
            {
                Id = 1,
                FontName = "Anonymous_Pro",
                DisplayText = "Old-Computer"
            });
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

        public void SelectedFontChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                SelectedFont = FontsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                SelectedFont = null;
            }
        }
    }
}
