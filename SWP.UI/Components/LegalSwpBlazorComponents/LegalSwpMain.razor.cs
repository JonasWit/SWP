using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMain : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public IServiceProvider ServiceProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Parameter]
        public string ActiveUserId { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MainStore.CleanUpStore();
        }

        private async void UpdateView()
        {
            using var scope = ServiceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(ActiveUserId);

            if (user.LockoutEnd is not null)
            {
                NavigationManager.NavigateTo(NavigationManager.BaseUri, true);
            }

            StateHasChanged();
        }
  
        protected override async Task OnInitializedAsync()
        {
            MainStore.EnableLoading("Wczytywanie...");

            base.OnInitialized();
            MainStore.AddStateChangeListener(UpdateView);
            await MainStore.Initialize(ActiveUserId);

            MainStore.DisableLoading();
        }

        public bool showSidebar = false;

        public void ShowHideSidebar() => showSidebar = !showSidebar;

        public bool showTopNav = false;

        public void ShowHideTopNav() => showTopNav = !showTopNav;
    }
}
