using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions;
using SWP.UI.Components.LegalSwpBlazorComponents.Dialogs;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager
{

    public class UserManagerState
    {
        public IdentityUser SelectedUser { get; set; }
    }

    [UIScopedService]
    public class UserManagerStore : StoreBase<UserManagerState>
    {
        private readonly GeneralViewModel _generalViewModel;
        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public UserManagerStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService, GeneralViewModel generalViewModel)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _generalViewModel = generalViewModel;
        }

        public Task Initialize()
        {
            RefreshSore();
            return Task.CompletedTask;
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SelectedUserChangeAction.SelectedUserChange:
                    var selectedUserChangeAction = (SelectedUserChangeAction)action;
                    SelectedUserChange(selectedUserChangeAction.Arg);
                    break;
                case RemoveRelationAction.RemoveRelation:
                    await RemoveRelation();
                    break;
                case ConfirmRemoveAllDataAction.ConfirmRemoveAllData:
                    ConfirmRemoveAllData();
                    break;
                default:
                    break;
            }
        }

        private void SelectedUserChange(object value)
        {
            var input = (string)value;
            if (value != null)
            {
                _state.SelectedUser = MainStore.GetState().User.RelatedUsers.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                _state.SelectedUser = null;
            }
        }

        private async Task RemoveRelation()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var profileClaim = MainStore.GetState().User.ProfileClaim;
                var selectedUser = MainStore.GetState().User.RelatedUsers.FirstOrDefault(x => x.Id == _state.SelectedUser.Id);
                var userToRemove = await userManager.FindByIdAsync(_state.SelectedUser.Id);

                var result = await userManager.RemoveClaimAsync(userToRemove, profileClaim);

                _state.SelectedUser = MainStore.GetState().User.User;
                await MainStore.RefreshRelatedUsers();

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Użytkownik: {userToRemove.UserName} został usunięty z profilu {profileClaim.Value}.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task RemoveProfileData()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClient = scope.ServiceProvider.GetRequiredService<DeleteClient>();
                //todo: uncomment this and test
                //await deleteClient.Delete(MainStore.GetState().User.Profile);

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Usunięto wszystkie dane powiązane z profilem: {MainStore.GetState().User.Profile}", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ConfirmRemoveAllData()
        {
            EnableLoading("Usuwanie Danych...");

            _dialogService.Open<GenericDialogPopup>("Uwaga! Usunięcie danych będzie nieodwracalne!",
                new Dictionary<string, object>()
                {
                    { "Title", "Potwierdź usunięcie danych" },
                    { "TaskToExecuteAsync", new Func<Task>(RemoveProfileData) },
                    { "Description", "This is sample Description" },
                },
                _generalViewModel.DefaultDialogOptions);

            DisableLoading();
        }

        public override void CleanUpStore()
        {

        }

        public async Task UpdateAccess()
        { 
        
        
        
        
        }

        private async Task RefreshDataLists()
        { 
        
        
        
        
        }
    }
}
