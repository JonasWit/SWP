using Microsoft.AspNetCore.Components;
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
        public UsersStore UsersStore { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
            UsersStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView);
            UsersStore.AddStateChangeListener(UpdateView);
            await UsersStore.Initialize();
        }

        #region Actions

        private void RowSelected(object arg) => ActionDispatcher.Dispatch(new UserRowSelectedAction { Arg = arg });

        private void RoleChanged(int arg) => ActionDispatcher.Dispatch(new RoleChangedAction { Arg = arg });

        private void DeleteClaimRow(Claim arg) => ActionDispatcher.Dispatch(new DeleteClaimRowAction { Arg = arg });

        private void AddApplicationClaim() => ActionDispatcher.Dispatch(new AddApplicationClaimAction());

        private void AddStatusClaim() => ActionDispatcher.Dispatch(new AddStatusClaimAction());

        private void AddProfileClaim() => ActionDispatcher.Dispatch(new AddProfileClaimAction());

        private void LockUser(bool arg) => ActionDispatcher.Dispatch(new LockUserAction { Arg = arg });

        private void DeleteUser(UserModel arg) => ActionDispatcher.Dispatch(new DeleteUserAction { Arg = arg });

        #endregion
    }
}
