using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClientJobs
    {
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ClientJobsStore ClientJobsStore { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateCleanView);
            ClientJobsStore.RemoveStateChangeListener(UpdateView);
            ClientJobsStore.CleanUpStore();
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        private void UpdateCleanView()
        {
            ClientJobsStore.CleanUpStore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateCleanView);
            ClientJobsStore.AddStateChangeListener(UpdateView);
            ClientJobsStore.Initialize();
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        public string ArchvizedJobsFilterValue;

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;



    }
}
