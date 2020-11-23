using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMain : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Parameter]
        public string ActiveUserId { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MainStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

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
