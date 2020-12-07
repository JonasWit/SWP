using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            _state.ActiveUserId = userId;

            _state.User.User = await userManager.FindByIdAsync(_state.ActiveUserId);
            _state.User.Claims = await userManager.GetClaimsAsync(_state.User.User) as List<Claim>;
            _state.User.Roles = await userManager.GetRolesAsync(_state.User.User) as List<string>;

            _logger.LogWarning("Admin Panel accessed by {userName}", _state.User.User.UserName);
        }

        protected override void HandleActions(IAction action)
        {

        }

        public void ShowErrorPage(Exception ex)
        {
            _errorStore.SetException(ex, _state.ActiveUserId, _state.User.UserName);
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

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {
    
        }
    }
}
