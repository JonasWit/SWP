using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.Database;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminCommunication : IDisposable
    {
        [Inject]
        public CommunicationStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private async void UpdateView()
        {
            await Store.GetRecipients();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            Store.AddStateChangeListener(UpdateView);
            await Store.GetRecipients();
        }

        #region Actions

        private void Send() => ActionDispatcher.Dispatch(new SendAction());

        private void FilterRecipients() => ActionDispatcher.Dispatch(new FilterAction());

        private void ClearSelection() => ActionDispatcher.Dispatch(new ClearSelectionAction());

        #endregion
    }
}
