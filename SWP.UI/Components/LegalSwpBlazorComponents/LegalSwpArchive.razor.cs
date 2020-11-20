using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Archive;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpArchive : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ArchiveStore ArchiveStore { get; set; }

        public string ArchvizedClientsFilterValue;


        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            ArchiveStore.RemoveStateChangeListener(UpdateView);
            ArchiveStore.CleanUpStore();
        }

        private void UpdateView()
        {
            ArchiveStore.RefreshData();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            ArchiveStore.AddStateChangeListener(UpdateView);
            ArchiveStore.Initialize();
        }

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;


    }
}
