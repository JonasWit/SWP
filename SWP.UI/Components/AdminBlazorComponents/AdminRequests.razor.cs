using Microsoft.AspNetCore.Components;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication;
using SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminRequests : IDisposable
    {
        [Inject]
        public RequestsStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            Store.GetRequests();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            Store.EnableLoading();

            Store.AddStateChangeListener(UpdateView);
            Store.GetRequests();
            Store.DisableLoading();
        }

        #region Actions

        private void AdminRequestMessageSelectedChange(object arg) => ActionDispatcher.Dispatch(new AdminRequestMessageSelectedChangeAction { Arg = (int)arg });

        private void AdminRequestSelectedChange(RequestViewModel arg) => ActionDispatcher.Dispatch(new AdminRequestSelectedChangeAction { Arg = arg });

        private void AdminSubmitNewRequestResponse(CreateRequest.RequestMessage arg) => ActionDispatcher.Dispatch(new AdminSubmitNewRequestResponseAction { Arg = arg });

        private void AdminUpdateRequestStatus() => ActionDispatcher.Dispatch(new AdminUpdateRequestStatusAction());

        #endregion
    }
}
