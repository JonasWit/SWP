using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.UI.BlazorApp;
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
        [CascadingParameter]
        public string ActiveUserId { get; set; }
        [Inject]
        public RequestCreateStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            await Store.Initialize(ActiveUserId);
            Store.AddStateChangeListener(UpdateView);
        }

        CompareOperator compareOperator = CompareOperator.GreaterThanEqual;

        #region Actions

        public void SelectedRequestSubjectChange(int? arg) => ActionDispatcher.Dispatch(new SelectedRequestReasonChangeAction { Arg = arg });

        public void SelectedRequestApplicationChange(int? arg) => ActionDispatcher.Dispatch(new SelectedRequestApplicationChangeAction { Arg = arg });

        public void SubmitNewRequest(CreateRequest.Request arg) => ActionDispatcher.Dispatch(new CreateNewRequestAction { Arg = arg });








        #endregion
    }
}
