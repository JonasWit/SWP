using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents.Requests
{
    public partial class RequestCreate : IDisposable
    {
        [Inject]
        public RequestCreateStore Store { get; set; }
        [Inject]
        public RequestsMainPanelStore MainStore { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            StateHasChanged();
        }
 
        protected override void OnInitialized()
        {
            Store.Initialize();
            MainStore.AddStateChangeListener(UpdateView);
            Store.AddStateChangeListener(UpdateView);
        }

        #region Actions

        public void SelectedRequestSubjectChange(int? arg) => ActionDispatcher.Dispatch(new SelectedRequestReasonChangeAction { Arg = arg });

        public void SelectedRequestApplicationChange(int? arg) => ActionDispatcher.Dispatch(new SelectedRequestApplicationChangeAction { Arg = arg });

        public void SubmitNewRequest(CreateRequest.Request arg) => ActionDispatcher.Dispatch(new CreateNewRequestAction { Arg = arg });

        #endregion
    }
}
