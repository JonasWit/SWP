using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using SWP.UI.BlazorApp.AdminApp.Stores.Error;
using SWP.UI.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Application
{
    public class ApplicationState
    {
        public NotificationService NotificationService { get; set; }
        public bool Loading { get; set; } = false;
        public string ActiveUserId { get; set; }
        public string LoadingMessage { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public AdminAppPanels ActivePanel { get; set; } = AdminAppPanels.Application;
    }

    [UIScopedService]
    public class ApplicationStore : StoreBase
    {
        private readonly ApplicationState _state;
        private readonly ErrorStore _errorStore;

        public ApplicationState GetState() => _state;

        public ApplicationStore(IServiceProvider serviceProvider, ErrorStore errorStore) : base(serviceProvider)
        {
            _state = new ApplicationState();
            _errorStore = errorStore;
        }

        public void SetActivePanel(AdminAppPanels panel) => _state.ActivePanel = panel;

        public async Task InitializeState(string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            _state.NotificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            _state.ActiveUserId = userId;

            _state.User.User = await userManager.FindByIdAsync(_state.ActiveUserId);
            _state.User.Claims = await userManager.GetClaimsAsync(_state.User.User) as List<Claim>;
            _state.User.Roles = await userManager.GetRolesAsync(_state.User.User) as List<string>;
        }

        public void ActivateLoading(string message)
        {
            _state.LoadingMessage = message;
            _state.Loading = true;
        }

        public void DeactivateLoading()
        {
            _state.LoadingMessage = "";
            _state.Loading = false;
        }

        public void ShowErrorPage(Exception ex)
        {
            _errorStore.SetException(ex);
            _state.ActivePanel = AdminAppPanels.Error;
            BroadcastStateChange();
        }

        public void DismissErrorPage()
        {
            _state.ActivePanel = AdminAppPanels.Application;
            BroadcastStateChange();
        }

        public void TestExceptionThrow()
        {
            try
            {
                throw new Exception("Test Exception");
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
        }
    }
}
