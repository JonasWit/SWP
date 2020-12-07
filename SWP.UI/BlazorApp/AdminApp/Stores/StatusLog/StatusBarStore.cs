using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.StatusLog
{
    public class StatusBarState
    {
        public string LogwindowContent { get; set; }
    }

    [UIScopedService]
    public class StatusBarStore : StoreBase<StatusBarState>
    {
        public StatusBarStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher)
            : base(serviceProvider, actionDispatcher) { }

        protected override void HandleActions(IAction action)
        {
  
        }


        public override void CleanUpStore()
        {
            _state.LogwindowContent = "";
        }

        public override void RefreshSore()
        {

        }
    }
}
