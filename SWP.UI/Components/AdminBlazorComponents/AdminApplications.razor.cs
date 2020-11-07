using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.ApplicationsOptions;
using SWP.UI.BlazorApp.AdminApp.Stores.Error;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminApplications
    {
        //[Parameter]
        //public AdminBlazorApp App { get; set; }
        //[Parameter]
        //public EventCallback<AdminBlazorApp> AppChanged { get; set; }

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
            ApplicationStore.AddStateChangeListener(UpdateView); //attach listener to the store
            ApplicationsOptionsStore.AddStateChangeListener(UpdateView);
            ApplicationsOptionsStore.InitializeState();
        }
    }
}
