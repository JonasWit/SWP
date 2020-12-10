using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests
{
    public class ProductivityState
    {
        public string TestMessage { get; set; } = "TEST";
    }

    [UIScopedService]
    public class RequestsStore : StoreBase<ProductivityState>
    {
        private readonly ILogger<ProductivityState> _logger;

        public RequestsStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<ProductivityState> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
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
