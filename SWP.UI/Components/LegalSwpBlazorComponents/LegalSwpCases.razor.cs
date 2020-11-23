using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpCases
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public CasesStore CasesStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateCleanView);
            CasesStore.RemoveStateChangeListener(UpdateView);
            CasesStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void UpdateCleanView()
        {
            CasesStore.CleanUpStore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            CasesStore.EnableLoading(CasesStore.DataLoadingMessage);

            MainStore.AddStateChangeListener(UpdateCleanView);
            CasesStore.AddStateChangeListener(UpdateView);
            CasesStore.Initialize();

            CasesStore.DisableLoading();
        }

        public bool showFirstSection = false;
        public void ShowHideFirstSection() => showFirstSection = !showFirstSection;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;

        public bool infoBoxVisibleI = false;
        public void ShowHideInfoBoxI() => infoBoxVisibleI = !infoBoxVisibleI;

        public bool infoBoxVisibleII = false;
        public void ShowHideInfoBoxII() => infoBoxVisibleII = !infoBoxVisibleII;

        public bool infoBoxVisibleIII = false;
        public void ShowHideInfoBoxIII() => infoBoxVisibleIII = !infoBoxVisibleIII;

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);
    }
}
