using SWP.Domain.Models.Log;
using SWP.UI.BlazorApp;
using SWP.UI.Components.AdminBlazorComponents.ViewModels;
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

        public ApplicationsPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;
            RefreshLog();
            return Task.CompletedTask;
        }

        public void RefreshLog() => LogRecords = new List<LogRecordViewModel>(GetLogs().Select(x => (LogRecordViewModel)x));

        public void RowSelected(object args) => SelectedLogRecord = (LogRecordViewModel)args;

        public async Task DeleteLogRecord(LogRecordViewModel logRecord)
        {
            try
            {



            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }
    }
}
