using Microsoft.AspNetCore.Components;
using SWP.Domain.Models.Portal;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.AdminLog;
using SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminLog : IDisposable
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Inject]
        public LogStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public IEnumerable<LogRecordType> LogLevels => Enum.GetValues(typeof(LogRecordType)).Cast<LogRecordType>();

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView);
            Store.AddStateChangeListener(UpdateView);
        }

        #region Actions

        private void RefreshLogs(object arg) => ActionDispatcher.Dispatch(new RefreshLogsAction());

        private void RefreshLogsForLastWeek(object arg) => ActionDispatcher.Dispatch(new RefreshLogsForLastWeekAction());

        private void DeleteLogRecord(Log arg) => ActionDispatcher.Dispatch(new DeleteLogRecordAction { Arg = arg });

        private void RowSelected(object arg) => ActionDispatcher.Dispatch(new LogRowSelectedAction { Arg = arg });

        private void SelectedLogTypesChange(object arg) => ActionDispatcher.Dispatch(new SelectedLogTypesChangeAction { Arg = arg });

        private void LogStartDateChange(object arg) => ActionDispatcher.Dispatch(new LogStartDateChangeAction { Arg = arg });

        #endregion
    }
}
