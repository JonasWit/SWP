using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.Clients;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClients : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ClientsStore ClientsStore { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            ClientsStore.RemoveStateChangeListener(UpdateView);
            ClientsStore.CleanUpStore();
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            MainStore.AddStateChangeListener(UpdateView);
            ClientsStore.AddStateChangeListener(UpdateView);
        }

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

        public bool clientListInfoVisible = false;
        public void ShowHideClientI() => clientListInfoVisible = !clientListInfoVisible;

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

        public bool showClientListVisible = false;
        public void ShowHideClientsList() => showClientListVisible = !showClientListVisible;
    }
}
