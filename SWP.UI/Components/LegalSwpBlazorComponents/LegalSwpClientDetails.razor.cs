using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClientDetails : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ClientDetailsStore ClientDetailsStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }

        [Inject]
        public TooltipService TooltipService { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MainStore.RemoveStateChangeListener(ResetSelections);
            ClientDetailsStore.RemoveStateChangeListener(UpdateView);
            ClientDetailsStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void ResetSelections() => ClientDetailsStore.ResetSelections();

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            MainStore.AddStateChangeListener(ResetSelections);
            ClientDetailsStore.AddStateChangeListener(UpdateView);
            ClientDetailsStore.Initialize();
        }

        public bool addClientformVisible = false;

        public bool contactsListInfoVisible = false;
        public void ShowHideContactsI() => contactsListInfoVisible = !contactsListInfoVisible;

        public bool showContactsListVisible = false;
        public void ShowHideContactsList() => showContactsListVisible = !showContactsListVisible;   
    }
}
