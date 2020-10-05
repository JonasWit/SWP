using Radzen;
using Radzen.Blazor;
using SWP.Domain.Models.Log;
using SWP.UI.BlazorApp;
using SWP.UI.Components.AdminBlazorComponents.ViewModels;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class ApplicationsPage : BlazorPageBase
    {
        public AdminBlazorApp App { get; set; }
        public LogRecordViewModel SelectedLogRecord { get; set; }
        public List<LogRecordViewModel> LogRecords { get; set; } = new List<LogRecordViewModel>();
        public RadzenGrid<LogRecordViewModel> LogGrid { get; set; }

        public ApplicationsPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;
            RefreshLog();
            return Task.CompletedTask;
        }

        public void RefreshLog() => LogRecords = new List<LogRecordViewModel>(GetLogs().Select(x => (LogRecordViewModel)x));

        public void RowSelected(object value)
        {
            var input = (LogRecordViewModel)value;
            if (value != null)
            {
                SelectedLogRecord = LogRecords.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                SelectedLogRecord = null;
            }
        }

        public void TestExceptionThrow()
        {
            try
            {
                throw new Exception("Test Exception");
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task DeleteLogRecord(LogRecordViewModel logRecord)
        {
            try
            {
                await DeleteLog(logRecord.Id);
                LogRecords.RemoveAll(x => x.Id == logRecord.Id);
                await LogGrid.Reload();

                if (SelectedLogRecord.Id == logRecord.Id)
                {
                    SelectedLogRecord = null;
                }
                    
                App.ShowNotification(NotificationSeverity.Warning, "Deleted!", $"Record: {logRecord.Message} has been deleted.", 5000);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }
    }
}
