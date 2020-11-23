using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.Finance;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpFinance : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public FinanceStore FinanceStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateCleanView);
            FinanceStore.RemoveStateChangeListener(UpdateView);
            FinanceStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void UpdateCleanView()
        {
            FinanceStore.CleanUpStore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateCleanView);
            FinanceStore.AddStateChangeListener(UpdateView);
            FinanceStore.Initialize();
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
    }
}
