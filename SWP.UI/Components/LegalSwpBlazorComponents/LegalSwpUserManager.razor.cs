using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.UserManager;
using SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpUserManager : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public UserManagerStore Store { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            Store.RemoveStateChangeListener(RefreshView);
        }

        private void RefreshView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            Store.EnableLoading(Store.DataLoadingMessage);

            MainStore.AddStateChangeListener(RefreshView);
            Store.AddStateChangeListener(RefreshView);

            Store.DisableLoading();
        }

        public string RelatedUsersFilterValue = "";

        #region Actions

        private void SelectedUserChange(object arg) => ActionDispatcher.Dispatch(new SelectedUserChangeAction { Arg = arg });

        private void RemoveRelation() => ActionDispatcher.Dispatch(new RemoveRelationAction());

        private void ConfirmRemoveAllData() => ActionDispatcher.Dispatch(new ConfirmRemoveAllDataAction());

        private void SelectedClientChange(object arg) => ActionDispatcher.Dispatch(new UserManagerSelectedClientChangeAction { Arg = arg });

        private void SelectedCaseChange(object arg) => ActionDispatcher.Dispatch(new UserManagerSelectedCaseChangeAction { Arg = arg });

        private void UpdateAccess() => ActionDispatcher.Dispatch(new UserManagerUpdateAccessAction());

        #endregion

    }
}
