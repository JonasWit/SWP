using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Models.Portal.Communication;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using SWP.UI.Utilities;
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
        public string ActiveUserName { get; set; }
        public int SelectedRequestId { get; set; }
        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();

        public enum InnerComponents
        {
            Info = 0,
            Create = 1,
            Details = 2,
            Error = 3
        }
    }

    [UIScopedService]
    public class RequestsMainPanelStore : StoreBase<RequestsMainPanelState>
    {
        private readonly ILogger<RequestsMainPanelState> _logger;
        private readonly NavigationManager _navigationManager;

        public RequestsMainPanelStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsMainPanelState> logger,
            NavigationManager navigationManager)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
            _navigationManager = navigationManager;
        }

        public void Initialize(string userId, string userName)
        {
            try
            {
                EnableLoading("Wczytywanie...");

                _state.ActiveUserId = userId;
                _state.ActiveUserName = userName;
                GetRequests();

                DisableLoading();
            }
            catch (Exception ex)
            {
                HandleError(ex, "Exception when initializing Contact Panel");
            }
        }

        public void HandleError(Exception ex, string logMessage)
        {
            _logger.LogError(ex, LogTags.PortalAppErrorLogPrefix + logMessage + " - Active User: {userName}", _state.ActiveUserName);
            _navigationManager.NavigateTo($@"{_navigationManager.BaseUri}Error", true);
        }

        protected override void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case GetAllRequestsWithoutDataAction.GetAllRequestsWithoutData:
                    GetRequests();
                    break;
                case ActivateNewRequestPanelAction.ActivateNewRequestPanel:
                    SetActiveComponent(RequestsMainPanelState.InnerComponents.Create);
                    break;
                case ActivateRequestInfoPanelAction.ActivateRequestInfoPanel:
                    SetActiveComponent(RequestsMainPanelState.InnerComponents.Info);
                    break;
                case RequestSelectedAction.RequestSelected:
                    var requestSelectedAction = (RequestSelectedAction)action;
                    ShowRequestDetails(requestSelectedAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void GetRequests()
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            var list = getRequest.GetRequestsForClient(_state.ActiveUserId);
            _state.Requests = list.Select(x => (RequestViewModel)x).ToList().OrderByDescending(x => x.Updated).ToList();
        }

        private void ShowRequestDetails(int id)
        {
            _state.SelectedRequestId = id;
            _state.CurrentComponent = RequestsMainPanelState.InnerComponents.Details;
            BroadcastStateChange();
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

        public override void RefreshSore()
        {
            GetRequests();
            BroadcastStateChange();
        }
    }
}
