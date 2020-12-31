using Microsoft.AspNetCore.Components;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Models.Portal;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Users;
using SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminUsers : IDisposable
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Inject]
        public UsersStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(StateHasChanged);
            Store.RemoveStateChangeListener(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(StateHasChanged);
            Store.AddStateChangeListener(StateHasChanged);
            await Store.Initialize();
        }

        public string SelectedClaim;

        #region Actions

        private void RowSelected(object arg) => ActionDispatcher.Dispatch(new UserRowSelectedAction { Arg = arg });

        private void RoleChanged(int arg) => ActionDispatcher.Dispatch(new RoleChangedAction { Arg = arg });

        private void DeleteClaimRow(Claim arg) => ActionDispatcher.Dispatch(new DeleteClaimRowAction { Arg = arg });

        private void AddApplicationClaim() => ActionDispatcher.Dispatch(new AddApplicationClaimAction());

        private void AddStatusClaim() => ActionDispatcher.Dispatch(new AddStatusClaimAction());

        private void AddProfileClaim() => ActionDispatcher.Dispatch(new AddProfileClaimAction());

        private void AddProfileClaimFromList() => ActionDispatcher.Dispatch(new AddProfileClaimFromListAction());

        private void SelectedProfileChange(object arg) => ActionDispatcher.Dispatch(new SelectedProfileChangeAction { Arg = arg });

        private void LockUser(bool arg) => ActionDispatcher.Dispatch(new LockUserAction { Arg = arg });

        private void DeleteUser(UserModel arg) => ActionDispatcher.Dispatch(new DeleteUserAction { Arg = arg });


        private void SubmitNewLicense(CreateLicense.Request arg) => ActionDispatcher.Dispatch(new SubmitNewLicenseAction { Arg = arg });

        private void OnUpdateLicenseRow(UserLicense arg) => ActionDispatcher.Dispatch(new OnUpdateLicenseRowAction { Arg = arg });

        private void EditLicenseRow(UserLicense arg) => ActionDispatcher.Dispatch(new EditLicenseRowAction { Arg = arg });

        private void SaveLicenseRow(UserLicense arg) => ActionDispatcher.Dispatch(new SaveLicenseRowAction { Arg = arg });

        private void CancelLicenseEdit(UserLicense arg) => ActionDispatcher.Dispatch(new CancelLicenseEditAction { Arg = arg });

        private void DeleteLicenseRow(UserLicense arg) => ActionDispatcher.Dispatch(new DeleteLicenseRowAction { Arg = arg });


        #endregion
    }
}
