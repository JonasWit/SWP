using Microsoft.Extensions.Logging;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests
{
    public class RequestsPanelDetailsState
    {
        public string TestMessage { get; set; } = "TEST";




        public class PresetTitle
        {
            public int MyProperty { get; set; }

        }


    }

    [UIScopedService]
    public class RequestsPanelDetailsStore : StoreBase<RequestsPanelDetailsState>
    {
        private readonly ILogger<RequestsPanelDetailsStore> _logger;

        public RequestsPanelDetailsStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsPanelDetailsStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void Initialize(string userId)
        {


        }

        protected override void HandleActions(IAction action)
        {
            switch (action)
            {
                default:
                    break;
            }
        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }
}
