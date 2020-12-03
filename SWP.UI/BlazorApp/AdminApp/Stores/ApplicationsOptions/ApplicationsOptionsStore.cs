using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.Components.AdminBlazorComponents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.ApplicationsOptions
{
    public class ApplicationOptionsState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public LogRecordViewModel SelectedLogRecord { get; set; }
        public List<LogRecordViewModel> LogRecords { get; set; } = new List<LogRecordViewModel>();
        public RadzenGrid<LogRecordViewModel> LogGrid { get; set; }
    }

    [UIScopedService]
    public class ApplicationsOptionsStore : StoreBase<ApplicationOptionsState>
    {
        public ApplicationsOptionsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
            : base(serviceProvider, actionDispatcher, notificationService) 
        {

        }

        public void Initialize()
        {
            using var scope = _serviceProvider.CreateScope();
            //todo:add logging!

            //var getLogRecords = scope.ServiceProvider.GetRequiredService<GetLogRecords>();

            RefreshLogs();
        }

        public void RefreshLogs()
        {
            //todo:add logging!

            using var scope = _serviceProvider.CreateScope();
            //var logs = scope.ServiceProvider.GetRequiredService<GetLogRecords>();

            //_state.LogRecords = new List<LogRecordViewModel>(_portalLogger.GetLogRecords().Select(x => (LogRecordViewModel)x));
            BroadcastStateChange();
        }

        public void ActivateLoading(string message)
        {
            _state.LoadingMessage = message;
            _state.Loading = true;
        }

        public void DeactivateLoading()
        {
            _state.LoadingMessage = "";
            _state.Loading = false;
        }

        public async Task DeleteLogRecord(LogRecordViewModel logRecord)
        {
            try
            {
                //todo:add logging!


                //using var scope = _serviceProvider.CreateScope();


                //_state.LogRecords.RemoveAll(x => x.Id == logRecord.Id);
                //await _state.LogGrid.Reload();

                //if (_state.SelectedLogRecord != null && _state.SelectedLogRecord.Id == logRecord.Id)
                //{
                //    _state.SelectedLogRecord = null;
                //}

                //BroadcastStateChange();

                //_notificationService.Notify(new NotificationMessage()
                //{
                //    Severity = NotificationSeverity.Warning,
                //    Summary = "Deleted!",
                //    Detail = $"Record: {logRecord.Message} has been deleted.",
                //    Duration = 3000
                //});
            }
            catch (Exception ex)
            {
                var appStore = _serviceProvider.GetRequiredService<ApplicationStore>();
                appStore.ShowErrorPage(ex);
            }
        }

        public async Task ShowErrorPage()
        {
            var apstore = _serviceProvider.GetRequiredService<ApplicationStore>();

            try
            {
                throw new Exception("Test Exception");
            }
            catch (Exception ex)
            {
                apstore.ShowErrorPage(ex);
            } 
        }

        public void RowSelected(object value)
        {
            var input = (LogRecordViewModel)value;
            if (value != null)
            {
                _state.SelectedLogRecord = _state.LogRecords.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedLogRecord = null;
            }
        }

        protected override void HandleActions(IAction action)
        {

        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }


}
