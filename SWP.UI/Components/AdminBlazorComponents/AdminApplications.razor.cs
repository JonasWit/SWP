using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.ApplicationsOptions;
using System;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminApplications : IDisposable
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Inject]
        public ApplicationsOptionsStore ApplicationsOptionsStore { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
            ApplicationsOptionsStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView);
            ApplicationsOptionsStore.AddStateChangeListener(UpdateView);
            ApplicationsOptionsStore.Initialize();
        }
    }
}
