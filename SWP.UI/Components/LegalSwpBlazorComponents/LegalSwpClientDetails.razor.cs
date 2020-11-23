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

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateCleanView);
            ClientDetailsStore.RemoveStateChangeListener(UpdateView);
            ClientDetailsStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void UpdateCleanView()
        {
            ClientDetailsStore.CleanUpStore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateCleanView);
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
