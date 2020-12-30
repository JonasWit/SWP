﻿using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using System;
using System.Threading.Tasks;

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
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            Store.Initialize(ActiveUserId, ActiveUserName);
            Store.AddStateChangeListener(UpdateView);
        }

        #region Actions

        private void ReuqestSelected(int id) => ActionDispatcher.Dispatch(new RequestSelectedAction { Arg = id });

        private void ActivateCreatePanel() => ActionDispatcher.Dispatch(new ActivateNewRequestPanelAction());

        private void ActivateHelpPanel() => ActionDispatcher.Dispatch(new ActivateRequestInfoPanelAction());






        #endregion
    }
}
