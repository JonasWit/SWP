using Microsoft.Extensions.Logging;
using System;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests
{
    public class ProductivityState
    {
        public string TestMessage { get; set; } = "TEST";




        public class PresetTitle
        {
            public int MyProperty { get; set; }

        }


    }

    [UIScopedService]
    public class RequestsPanelStore : StoreBase<ProductivityState>
    {
        private readonly ILogger<ProductivityState> _logger;

        public RequestsPanelStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<ProductivityState> logger)
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
