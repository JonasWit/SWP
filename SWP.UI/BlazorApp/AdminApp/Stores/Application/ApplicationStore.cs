using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using SWP.UI.BlazorApp.AdminApp.Stores.Error;
using SWP.UI.Models;
using SWP.UI.Utilities;
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
        public string LoadingMessage { get; set; }
        public AppActiveUserManager AppActiveUserManager { get; set; } 
        public AdminAppPanels ActivePanel { get; set; } = AdminAppPanels.Users;
    }

    [UIScopedService]
    public class ApplicationStore : StoreBase<ApplicationState>
    {
        private readonly ErrorStore _errorStore;
        private readonly ILogger<ApplicationStore> _logger;

        public ApplicationStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, ErrorStore errorStore, ILogger<ApplicationStore> logger) 
            : base(serviceProvider, actionDispatcher)
        {
            _errorStore = errorStore;
            _logger = logger;
        }

        public void SetActivePanel(AdminAppPanels panel) => _state.ActivePanel = panel;

        public async Task Initialize(string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            _state.NotificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            _state.AppActiveUserManager = new AppActiveUserManager(_serviceProvider, userId);
            await _state.AppActiveUserManager.UpdateClaimsAndRoles();

            _logger.LogInformation(LogTags.AdminAppLogPrefix + "Admin Application accessed by user {userName}", _state.AppActiveUserManager.User.UserName);
        }

        protected override void HandleActions(IAction action)
        {

        }

        public void ShowErrorPage(Exception ex)
        {
            _errorStore.SetException(ex, _state.AppActiveUserManager.UserId, _state.AppActiveUserManager.UserName);
            _state.ActivePanel = AdminAppPanels.Error;
            BroadcastStateChange();
        }

        public void DismissErrorPage()
        {
            _state.ActivePanel = AdminAppPanels.Log;
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

        public void RefreshMainComponent() => BroadcastStateChange();
    }
}
