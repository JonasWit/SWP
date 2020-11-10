using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Users;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminUsers : IDisposable
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Inject]
        public UsersStore UsersStore { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
            UsersStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView); //attach listener to the store
            UsersStore.AddStateChangeListener(UpdateView);
            await UsersStore.InitializeState();
        }

        public string ProfilesFilterValue = "";
    }
}
