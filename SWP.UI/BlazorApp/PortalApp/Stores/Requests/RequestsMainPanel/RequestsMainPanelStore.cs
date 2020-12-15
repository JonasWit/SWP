using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel
{
    public class RequestsMainPanelState
    {
        public InnerComponents CurrentComponent { get; set; } = InnerComponents.Info;
        public string ActiveUserId { get; set; }
        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();


        public class PresetTitle
        {
            public int MyProperty { get; set; }

        }

        public enum InnerComponents
        { 
            Info = 0,
            Create = 1,
            Details = 2   
        }
    }

    [UIScopedService]
    public class RequestsMainPanelStore : StoreBase<RequestsMainPanelState>
    {
        private readonly ILogger<RequestsMainPanelState> _logger;


        public RequestsMainPanelStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsMainPanelState> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public async Task Initialize(string userId)
        {
            _state.ActiveUserId = userId;
            await GetRequests();
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case GetAllRequestsWithoutDataAction.GetAllRequestsWithoutData:
                    await GetRequests();
                    break;
                default:
                    break;
            }
        }

        private async Task GetRequests()
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            var list = await getRequest.GetRequestsForClient(_state.ActiveUserId);
            _state.Requests = list.Select(x => (RequestViewModel)x).ToList();
        }

        public void SetActiveComponent(RequestsMainPanelState.InnerComponents component)
        {
            _state.CurrentComponent = component;
            RefreshSore();
            BroadcastStateChange();    
        }

        public override void CleanUpStore()
        {

        }

        public override async void RefreshSore()
        {
            await GetRequests();
            BroadcastStateChange();
        }
    }
}
