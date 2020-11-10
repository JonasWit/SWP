using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.Database;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminDatabase : IDisposable
    {
        [Inject]
        public DatabaseStore DatabaseStore { get; set; }

        public void Dispose()
        {
            DatabaseStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            DatabaseStore.AddStateChangeListener(UpdateView);
            await DatabaseStore.InitializeState();
        }
    }
}
