using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Database;
using SWP.UI.BlazorApp.AdminApp.Stores.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminPortal : IDisposable
    {
        [Inject]
        public PortalStore Store { get; set; }
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
    }
}
