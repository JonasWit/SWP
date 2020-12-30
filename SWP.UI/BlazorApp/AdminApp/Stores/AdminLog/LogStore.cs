using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.Logs;
using SWP.Domain.Models.Portal;
using SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.StatusLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog
{
    public enum LogRecordType
    {
        Verbose = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5,
    }

    public class ApplicationOptionsState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public Log SelectedLogRecord { get; set; }
        public List<Log> LogRecords { get; set; } = new List<Log>();
        public RadzenGrid<Log> LogGrid { get; set; }
        public IEnumerable<int> SelectedLogRecordTypes { get; set; }
        public DateTime? LogStartDate { get; set; }
        public DateTime? LogEndDate { get; set; }
    }

    [UIScopedService]
    public class LogStore : StoreBase<ApplicationOptionsState>
    {
        ApplicationStore AppStore => _serviceProvider.GetRequiredService<ApplicationStore>();

        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();

        public LogStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
            : base(serviceProvider, actionDispatcher, notificationService)
        {

        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case RefreshLogsAction.RefreshLogs:
                    RefreshLogs();
                    break;
                case DeleteLogRecordAction.DeleteLogRecord:
                    var deleteLogRecordAction = (DeleteLogRecordAction)action;
                    await DeleteLogRecord(deleteLogRecordAction.Arg);
                    break;
                case LogRowSelectedAction.LogRowSelected:
                    var rowSelectedAction = (LogRowSelectedAction)action;
                    RowSelected(rowSelectedAction.Arg);
                    break;
                case SelectedLogTypesChangeAction.SelectedLogTypesChange:
                    var selectedLogTypesChangeAction = (SelectedLogTypesChangeAction)action;
                    SelectedLogTypesChange(selectedLogTypesChangeAction.Arg);
                    break;
                case LogStartDateChangeAction.LogStartDateChange:
                    var logStartDateChangeAction = (LogStartDateChangeAction)action;
                    LogStartDateChange(logStartDateChangeAction.Arg);
                    break;
                case RefreshLogsForLastWeekAction.RefreshLogsForLastWeek:
                    RefreshLogsLastWeek();
                    break;
                default:
                    break;
            }
        }

        #region Actions

        private void RefreshLogs()
        {
            try
            {
                Loading = true;

                if (_state.LogStartDate == null || _state.LogEndDate == null)
                {
                    ShowNotification(NotificationSeverity.Error, "Aborted!", $"Select Time Period!", 2000);
                    return;
                }

                if (_state.SelectedLogRecordTypes == null || _state.SelectedLogRecordTypes.Count() == 0)
                {
                    ShowNotification(NotificationSeverity.Error, "Aborted!", $"Select Record Types!", 2000);
                    return;
                }

                if (_state.LogEndDate < _state.LogStartDate)
                {
                    var startDate = (DateTime)_state.LogStartDate;
                    _state.LogEndDate = startDate.AddDays(1);
                    ShowNotification(NotificationSeverity.Warning, "Warning!", $"End Date set to Start Date +1 Day!", 2000);
                }

                using var scope = _serviceProvider.CreateScope();
                var getLogs = _serviceProvider.GetRequiredService<GetLogRecords>();

                var selectedTypes = Enum.GetValues(typeof(LogRecordType)).Cast<LogRecordType>()
                    .Where(x => _state.SelectedLogRecordTypes.Contains((int)x))
                    .Select(x => x.ToString())
                    .ToList();

                _state.LogRecords = getLogs.GetLogs(selectedTypes, (DateTime)_state.LogStartDate, (DateTime)_state.LogEndDate).OrderByDescending(x => x.TimeStamp).ToList();

                ShowNotification(NotificationSeverity.Success, "Success!", $"{_state.LogRecords.Count} Records downloaded", 2000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
            finally
            {
                Loading = false;
            }
        }

        private void RefreshLogsLastWeek()
        {
            try
            {
                Loading = true;

                if (_state.SelectedLogRecordTypes == null || _state.SelectedLogRecordTypes.Count() == 0)
                {
                    ShowNotification(NotificationSeverity.Error, "Aborted!", $"Select Record Types!", 2000);
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var getLogs = _serviceProvider.GetRequiredService<GetLogRecords>();

                var selectedTypes = Enum.GetValues(typeof(LogRecordType)).Cast<LogRecordType>()
                    .Where(x => _state.SelectedLogRecordTypes.Contains((int)x))
                    .Select(x => x.ToString())
                    .ToList();

                _state.LogRecords = getLogs.GetLogs(selectedTypes, DateTime.Now.AddDays(-7), DateTime.Now).OrderByDescending(x => x.TimeStamp).ToList();

                ShowNotification(NotificationSeverity.Success, "Success!", $"{_state.LogRecords.Count} Records downloaded", 2000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task DeleteLogRecord(Log logRecord)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var delete = _serviceProvider.GetRequiredService<DeleteLogRecord>();

                await delete.Delete(logRecord.Id);
                _state.LogRecords.RemoveAll(x => x.Id == logRecord.Id);
                await _state.LogGrid.Reload();

                if (_state.SelectedLogRecord != null && _state.SelectedLogRecord.Id == logRecord.Id)
                {
                    _state.SelectedLogRecord = null;
                }

                ShowNotification(NotificationSeverity.Success, "Success!", $"Records Deleted!", 2000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        private void RowSelected(object value)
        {
            var input = (Log)value;
            if (value != null)
            {
                _state.SelectedLogRecord = _state.LogRecords.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedLogRecord = null;
            }
        }

        private void SelectedLogTypesChange(object value)
        {
            _state.SelectedLogRecordTypes = value as IEnumerable<int>;
        }

        private void LogStartDateChange(object value) => _state.LogStartDate = (DateTime?)value;

        public void LogEndDateChange(object value) => _state.LogEndDate = (DateTime?)value;

        #endregion

        private void ShowErrorPage(Exception ex) => AppStore.ShowErrorPage(ex);

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }


}
