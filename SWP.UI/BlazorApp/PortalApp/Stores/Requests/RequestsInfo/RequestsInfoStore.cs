using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsInfo.Actions;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsInfo
{
    public class RequestsInfoState
    {



    }

    [UIScopedService]
    public class RequestsInfoStore : StoreBase<RequestsInfoState>
    {
        private readonly ILogger<RequestsInfoStore> _logger;
        public RequestsMainPanelStore MainStore => _serviceProvider.GetRequiredService<RequestsMainPanelStore>();

        public RequestsInfoStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsInfoStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void Initialize(string userId)
        {


        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case InfoCreateRequestAction.InfoCreateRequest:
                    await ShowCreateRequestPanel();
                    break;
                default:
                    break;
            }
        }

        private async Task ShowCreateRequestPanel() => await MainStore.SetActiveComponent(RequestsMainPanelState.InnerComponents.Create);
    }
}
