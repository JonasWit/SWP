using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp.LegalApp.Services.Reporting;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity
{
    public class ProductivityState
    {
        public CreateTimeRecord.Request NewTimeRecord { get; set; } = new CreateTimeRecord.Request();
        public LegalTimeSheetReport.ReportData NewTimesheetReport { get; set; } = new LegalTimeSheetReport.ReportData();

        public RadzenGrid<TimeRecordViewModel> TimeRecordsGrid { get; set; }
        public FontFilterRecord SelectedFont { get; set; }
        public List<FontFilterRecord> FontsFilterData { get; set; } = new List<FontFilterRecord>();

        public class FontFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText { get; set; }
            public string FontName { get; set; }
        }

        public MonthFilterRecord SelectedMonth { get; set; }
        public List<MonthFilterRecord> MonthsFilterData { get; set; } = new List<MonthFilterRecord>();

        public class MonthFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText => $"{Month}-{Year}";
            public int Month { get; set; }
            public int Year { get; set; }
        }
    }


    [UIScopedService]
    public class ProductivityStore : StoreBase
    {
        private readonly ProductivityState _state;

        public ProductivityState GetState() => _state;

        private MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ProductivityStore(IServiceProvider serviceProvider, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, notificationService, dialogService)
        {
            _state = new ProductivityState();
        }

        public void Initialize()
        {
            GetDataForMonthFilter();
            GetDataForFontFilter();
        }

        public void EditTimeRecordRow(TimeRecordViewModel time) => _state.TimeRecordsGrid.EditRow(time);

        public async Task AddRecordRowToCashMovements(TimeRecordViewModel time)
        {
            if (time.Total == 0)
            {
                ShowNotification(NotificationSeverity.Warning, "Błąd!", $"Nie można przenieść do Finansów wejscia, z kwotą 0 zł", GeneralViewModel.NotificationDuration);
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
                    UpdatedBy = _mainStore.GetState().User.UserName
                };

                var result = await createCashMovement.Create(_mainStore.GetState().ActiveClient.Id, _mainStore.GetState().User.Profile, request);

                _mainStore.AddCashMovementToActiveClient(result);

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana do Panelu Finanse", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
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
                    UpdatedBy = _mainStore.GetState().User.UserName
                });

                _mainStore.ReplaceTimeRecordFromActiveClient(result);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SaveTimeRecordRow(TimeRecordViewModel time) => _state.TimeRecordsGrid.UpdateRow(time);

        public void CancelTimeRecordEdit(TimeRecordViewModel time)
        {
            _state.TimeRecordsGrid.CancelEditRow(time);
            _mainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteTimeRecord = scope.ServiceProvider.GetRequiredService<DeleteTimeRecord>();

                await deleteTimeRecord.Delete(time.Id);

                _mainStore.RemoveTimeRecordFromActiveClient(time.Id);
                _mainStore.SetSelectedTimeRecord(null);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Wpis: {time.Name}, został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public async Task SubmitNewTimeRecord(CreateTimeRecord.Request request)
        {
            if (request.RecordedTime == new TimeSpan(0, 0, 0))
            {
                ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Nie moża dodać wpisu bez zarejestrowanego czasu!", GeneralViewModel.NotificationDuration);
                return;
            }

            if (request.Rate == 0)
            {
                ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Nie moża dodać wpisu bez określenia stawki!", GeneralViewModel.NotificationDuration);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createTimeRecord = scope.ServiceProvider.GetRequiredService<CreateTimeRecord>();

                request.UpdatedBy = _mainStore.GetState().User.UserName;

                var result = await createTimeRecord.Create(_mainStore.GetState().ActiveClient.Id, _mainStore.GetState().User.Profile, request);
                _state.NewTimeRecord = new CreateTimeRecord.Request();

                _mainStore.AddTimeRecordToActiveClient(result);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void ActiveTimeRecordChange(object value)
        {
            var input = (TimeRecordViewModel)value;
            if (value != null)
            {
                _mainStore.SetSelectedTimeRecord(_mainStore.GetState().ActiveClient.TimeRecords.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                _mainStore.SetSelectedTimeRecord(null);
            }
        }

        public void GetDataForMonthFilter()
        {
            int id = 1;
            _state.MonthsFilterData.Clear();

            foreach (var record in _mainStore.GetState().ActiveClient.TimeRecords)
            {
                var year = record.EventDate.Year;
                var month = record.EventDate.Month;

                if (!_state.MonthsFilterData.Any(x => x.Month == month && x.Year == year))
                {
                    _state.MonthsFilterData.Add(new ProductivityState.MonthFilterRecord { Id = id, Month = month, Year = year });
                    id++;
                }
            }

            if (_state.MonthsFilterData.Count != 0)
            {
                _state.MonthsFilterData.OrderBy(x => x.Year + x.Month);
            }
        }

        public void GetDataForFontFilter()
        {
            _state.FontsFilterData.Clear();

            _state.FontsFilterData.Add(new ProductivityState.FontFilterRecord
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
                _state.SelectedMonth = _state.MonthsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                _state.SelectedMonth = null;
            }
        }

        public void SelectedFontChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                _state.SelectedFont = _state.FontsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                _state.SelectedFont = null;
            }
        }

















    }

}
