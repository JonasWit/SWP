using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsInfo;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsInfo.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents.Requests
{
    public partial class Info : IDisposable
    {
        [Parameter]
        public string ActiveUserId { get; set; }
        [Inject]
        public RequestsInfoStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            Store.Initialize(ActiveUserId);
            Store.AddStateChangeListener(UpdateView);
        }

        #region Actions

        public void CreateRequest() => ActionDispatcher.Dispatch(new InfoCreateRequestAction());






        #endregion
    }
}
