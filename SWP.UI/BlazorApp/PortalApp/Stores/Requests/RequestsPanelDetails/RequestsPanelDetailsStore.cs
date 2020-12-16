using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Models.Portal.Communication;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests
{
    public class RequestsPanelDetailsState
    {
        public RequestViewModel ActiveRequest { get; set; } = new RequestViewModel();
    }

    [UIScopedService]
    public class RequestsPanelDetailsStore : StoreBase<RequestsPanelDetailsState>
    {
        private readonly ILogger<RequestsPanelDetailsStore> _logger;
        public RequestsMainPanelStore MainStore => _serviceProvider.GetRequiredService<RequestsMainPanelStore>();

        public RequestsPanelDetailsStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsPanelDetailsStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            _state.ActiveRequest = GetRequest(MainStore.GetState().SelectedRequestId);
        }

        private ClientRequest GetRequest(int requestId)
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            return getRequest.GetRequestWithMessages(requestId);
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
            _state.ActiveRequest = GetRequest(MainStore.GetState().SelectedRequestId);
        }
    }
}
