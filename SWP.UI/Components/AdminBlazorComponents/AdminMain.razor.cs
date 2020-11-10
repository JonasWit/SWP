using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminMain : IDisposable
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Parameter]
        public string ActiveUserId { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView);
            await ApplicationStore.InitializeState(ActiveUserId);
        }

        private bool hamburgerNavActive;
        private void ActivateNavBurger() => hamburgerNavActive = !hamburgerNavActive;
    }
}
