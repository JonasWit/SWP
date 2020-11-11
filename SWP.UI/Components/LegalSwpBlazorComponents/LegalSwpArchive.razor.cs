using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Archive;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
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




    }
}
