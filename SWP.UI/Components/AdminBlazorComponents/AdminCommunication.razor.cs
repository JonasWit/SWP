using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.Database;
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

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
            Store.AddStateChangeListener(UpdateView);
        }

        #region Actions

        private void Send() => ActionDispatcher.Dispatch(new SendAction());

        #endregion

    }
}
