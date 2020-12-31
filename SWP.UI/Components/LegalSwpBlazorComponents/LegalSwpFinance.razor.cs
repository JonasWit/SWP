using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.Application.LegalSwp.CashMovements;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Finance;
using SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions;
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
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            FinanceStore.RemoveStateChangeListener(UpdateView);
            FinanceStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void RefreshView()
        {
            if (MainStore.GetState().ActiveClient == null)
            {
                MainStore.SetActivePanel(LegalAppPanels.MyApp);
                return;
            }

            FinanceStore.CleanUpStore();
            FinanceStore.RefreshSore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(RefreshView);
            FinanceStore.AddStateChangeListener(UpdateView);
            FinanceStore.Initialize();
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        public bool infoBoxVisibleI = false;
        public void ShowHideInfoBoxI() => infoBoxVisibleI = !infoBoxVisibleI;

        public bool infoBoxVisibleII = false;
        public void ShowHideInfoBoxII() => infoBoxVisibleII = !infoBoxVisibleII;

        #region Actions

        private void EditCashMovementRow(CashMovementViewModel arg) => ActionDispatcher.Dispatch(new EditCashMovementRowAction { Arg = arg });

        private void OnUpdateCashMovementRow(CashMovementViewModel arg) => ActionDispatcher.Dispatch(new OnUpdateCashMovementRowAction { Arg = arg });

        private void SaveCashMovementRow(CashMovementViewModel arg) => ActionDispatcher.Dispatch(new SaveCashMovementRowAction { Arg = arg });

        private void CancelCashMovementEdit(CashMovementViewModel arg) => ActionDispatcher.Dispatch(new CancelCashMovementEditAction { Arg = arg });

        private void DeleteCashMovementRow(CashMovementViewModel arg) => ActionDispatcher.Dispatch(new DeleteCashMovementRowAction { Arg = arg });

        private void SubmitNewCashMovement(CreateCashMovement.Request arg) => ActionDispatcher.Dispatch(new SubmitNewCashMovementAction { Arg = arg });

        private void ActiveCashMovementChange(object arg) => ActionDispatcher.Dispatch(new ActiveCashMovementChangeAction { Arg = arg });

        private void SelectedMonthChange(object arg) => ActionDispatcher.Dispatch(new SelectedMonthChangeFinanceAction { Arg = arg });

        #endregion
    }
}
