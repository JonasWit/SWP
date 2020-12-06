using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.AdminLog;
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
            Store.Initialize();
        }
    }
}
