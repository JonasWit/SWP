using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;

namespace SWP.UI.Components.PortalBlazorComponents.Requests
{
    public partial class RequestsMainPanel : IDisposable
    {
        [Parameter]
        public string ActiveUserId { get; set; }
        [Parameter]
        public string ActiveUserName { get; set; }
        [Inject]
        public RequestsMainPanelStore Store { get; set; }
        [Inject]
        public RequestsPanelDetailsStore DetailsStore { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
            DetailsStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            Store.Initialize(ActiveUserId, ActiveUserName);
            Store.AddStateChangeListener(UpdateView);
            DetailsStore.AddStateChangeListener(UpdateView);
        }

        #region Actions

        private void RequestSelected(RequestViewModel arg) => ActionDispatcher.Dispatch(new RequestSelectedAction { Arg = arg });

        private void ActivateCreatePanel() => ActionDispatcher.Dispatch(new ActivateNewRequestPanelAction());

        private void ActivateHelpPanel() => ActionDispatcher.Dispatch(new ActivateRequestInfoPanelAction());






        #endregion
    }
}
