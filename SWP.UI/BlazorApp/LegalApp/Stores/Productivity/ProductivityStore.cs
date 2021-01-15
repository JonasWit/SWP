using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp.LegalApp.Services.Reporting;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions;
using SWP.UI.Components.ViewModels.LegalApp;
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
        public TimeRecordViewModel SelectedTimeRecord { get; set; }
        public List<TimeRecordViewModel> TimeRecords { get; set; } = new List<TimeRecordViewModel>();

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
    public class ProductivityStore : StoreBase<ProductivityState>
    {
        private MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ProductivityStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {

        }

        public void Initialize()
        {
            GetTimeRecords(MainStore.GetState().ActiveClient.Id);
            GetDataForMonthFilter();
            GetDataForFontFilter();
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case EditTimeRecordRowAction.EditTimeRecordRow:
                    var editTimeRecordRowAction = (EditTimeRecordRowAction)action;
                    EditTimeRecordRow(editTimeRecordRowAction.Arg);
                    break;
                case AddRecordRowToCashMovementsAction.AddRecordRowToCashMovements:
                    var addRecordRowToCashMovementsAction = (AddRecordRowToCashMovementsAction)action;
                    await AddRecordRowToCashMovements(addRecordRowToCashMovementsAction.Arg);
                    break;
                case OnUpdateTimeRecordRowAction.OnUpdateTimeRecordRow:
                    var onUpdateTimeRecordRowAction = (OnUpdateTimeRecordRowAction)action;
                    await OnUpdateTimeRecordRow(onUpdateTimeRecordRowAction.Arg);
                    break;
                case SaveTimeRecordRowAction.SaveTimeRecordRow:
                    var saveTimeRecordRowAction = (SaveTimeRecordRowAction)action;
                    SaveTimeRecordRow(saveTimeRecordRowAction.Arg);
                    break;
                case CancelTimeRecordEditAction.CancelTimeRecordEdit:
                    var cancelTimeRecordEditAction = (CancelTimeRecordEditAction)action;
                    CancelTimeRecordEdit(cancelTimeRecordEditAction.Arg);
                    break;
                case DeleteTimeRecordRowAction.DeleteTimeRecordRow:
                    var deleteTimeRecordRowAction = (DeleteTimeRecordRowAction)action;
                    await DeleteTimeRecordRow(deleteTimeRecordRowAction.Arg);
                    break;
                case SubmitNewTimeRecordAction.SubmitNewTimeRecord:
                    var submitNewTimeRecordAction = (SubmitNewTimeRecordAction)action;
                    await SubmitNewTimeRecord(submitNewTimeRecordAction.Arg);
                    break;
                case ActiveTimeRecordChangeAction.ActiveTimeRecordChange:
                    var activeTimeRecordChangeAction = (ActiveTimeRecordChangeAction)action;
                    ActiveTimeRecordChange(activeTimeRecordChangeAction.Arg);
                    break;
                case SelectedMonthChangeProductivityAction.SelectedMonthChange:
                    var selectedMonthChangeAction = (SelectedMonthChangeProductivityAction)action;
                    SelectedMonthChange(selectedMonthChangeAction.Arg);
                    break;
                case SelectedFontChangeAction.SelectedFontChange:
                    var selectedFontChangeAction = (SelectedFontChangeAction)action;
                    SelectedFontChange(selectedFontChangeAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void GetTimeRecords(int clientId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getTimeRecords = scope.ServiceProvider.GetRequiredService<GetTimeRecords>();

                _state.TimeRecords = getTimeRecords.Get(clientId).Select(x => (TimeRecordViewModel)x).ToList();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        public string GetTimeSpent()
        {
            if (_state.TimeRecords.Count == 0) return "Brak zarejestrowanego czasu.";

            var spentTime = new TimeSpan(_state.TimeRecords.Sum(x => x.RecordedHours), _state.TimeRecords.Sum(x => x.RecordedMinutes), 0);
            var result = $"Poświęcony czas: {spentTime.Days} dni {spentTime.Hours} godz. {spentTime.Minutes} min.";

            return result;
        }

        private void GetDataForMonthFilter()
        {
            int id = 1;
            _state.MonthsFilterData.Clear();

            foreach (var record in _state.TimeRecords)
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

        private void GetDataForFontFilter()
        {
            _state.FontsFilterData.Clear();

            _state.FontsFilterData.Add(new ProductivityState.FontFilterRecord
            {
                Id = 1,
                FontName = "Anonymous_Pro",
                DisplayText = "Old-Computer"
            });
        }

        private void SetSelectedTimeRecord(TimeRecordViewModel entity) => _state.SelectedTimeRecord = entity;

        private void AddTimeRecord(TimeRecordViewModel entity) => _state.TimeRecords.Add(entity);

        private void ReplaceTimeRecord(TimeRecordViewModel entity) => _state.TimeRecords[_state.TimeRecords.FindIndex(x => x.Id == entity.Id)] = entity;

        private void RemoveTimeRecord(int id) => _state.TimeRecords.RemoveAll(x => x.Id == id);

        #region Actions

        private void EditTimeRecordRow(TimeRecordViewModel time) => _state.TimeRecordsGrid.EditRow(time);

        private async Task AddRecordRowToCashMovements(TimeRecordViewModel time)
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
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                };

                var result = await createCashMovement.Create(MainStore.GetState().ActiveClient.Id, MainStore.GetState().AppActiveUserManager.ProfileName, request);

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana do Panelu Finanse", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task OnUpdateTimeRecordRow(TimeRecordViewModel time)
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
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                ReplaceTimeRecord(result);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został zmieniony.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveTimeRecordRow(TimeRecordViewModel time) => _state.TimeRecordsGrid.UpdateRow(time);

        private void CancelTimeRecordEdit(TimeRecordViewModel time)
        {
            _state.TimeRecordsGrid.CancelEditRow(time);
            BroadcastStateChange();
        }

        private async Task DeleteTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteTimeRecord = scope.ServiceProvider.GetRequiredService<DeleteTimeRecord>();

                await deleteTimeRecord.Delete(time.Id);

                RemoveTimeRecord(time.Id);
                SetSelectedTimeRecord(null);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Wpis: {time.Name}, został usunięty.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task SubmitNewTimeRecord(CreateTimeRecord.Request request)
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

                request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

                var result = await createTimeRecord.Create(MainStore.GetState().ActiveClient.Id, MainStore.GetState().AppActiveUserManager.ProfileName, request);
                _state.NewTimeRecord = new CreateTimeRecord.Request();

                AddTimeRecord(result);

                await _state.TimeRecordsGrid.Reload();
                GetDataForMonthFilter();

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został dodany.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ActiveTimeRecordChange(object value)
        {
            var input = (TimeRecordViewModel)value;
            if (value != null)
            {
                SetSelectedTimeRecord(_state.TimeRecords.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                SetSelectedTimeRecord(null);
            }
        }

        private void SelectedMonthChange(object value)
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

        private void SelectedFontChange(object value)
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

        #endregion

        public void CleanUpStore()
        {
            _state.SelectedMonth = null;
            _state.SelectedTimeRecord = null;
        }

        public void RefreshSore()
        {
            GetTimeRecords(MainStore.GetState().ActiveClient.Id);
            GetDataForMonthFilter();
            GetDataForFontFilter();
        }
    }
}
