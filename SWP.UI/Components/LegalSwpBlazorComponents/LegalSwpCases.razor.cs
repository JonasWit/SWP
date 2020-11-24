using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.Application.LegalSwp.Cases;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
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
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            CasesStore.RemoveStateChangeListener(UpdateView);
            CasesStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void RefreshView()
        {
            if (MainStore.GetState().ActiveClient == null)
            {
                MainStore.SetActivePanel(LegalAppPanels.MyApp);
                return;
            }

            CasesStore.CleanUpStore();
            CasesStore.RefreshSore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            CasesStore.EnableLoading(CasesStore.DataLoadingMessage);

            MainStore.AddStateChangeListener(RefreshView);
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

        #region Actions 

        private void CreateNewCase(CreateCase.Request request) => ActionDispatcher.Dispatch(new CreateNewCaseAction { Request = request });

        private void EditCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new EditCaseRowAction { Arg = data });

        #endregion

    }
}
