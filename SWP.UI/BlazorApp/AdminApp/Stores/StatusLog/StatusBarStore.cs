using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.StatusLog
{
    public class StatusBarState
    {
        public string LogWindowContent { get; set; }
    }

    [UIScopedService]
    public class StatusBarStore : StoreBase<StatusBarState>
    {
        public StatusBarStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher)
            : base(serviceProvider, actionDispatcher) { }

        protected override void HandleActions(IAction action)
        {
  
        }

        public void UpdateLogWindow(string message)
        {
            var log = _state.LogWindowContent;
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} : {message}";

            if (string.IsNullOrEmpty(log) || !log.Contains(logMessage))
            {
                _state.LogWindowContent = logMessage + Environment.NewLine + log;
            }
            
            BroadcastStateChange();
        }

        public override void CleanUpStore()
        {
            _state.LogWindowContent = "";
        }

        public override void RefreshSore()
        {

        }
    }
}
