using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SWP.UI.BlazorApp.AdminApp.Stores.ApplicationStore.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.ApplicationStore
{
    public class ApplicationState
    {
        public bool Loading { get; set; } = false;
        public string ActiveUserId { get; set; }
        public string LoadingMessage { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public AdminAppPanels ActivePanel { get; set; } = AdminAppPanels.Application;
    }

    public class ApplicationStore : StoreBase
    {
        private ApplicationState _state;

        public ApplicationState GetState() => _state;

        public ApplicationStore(IActionDispatcher actionDispatcher, IServiceProvider serviceProvider) : base(actionDispatcher, serviceProvider)
        {

        }

        protected override void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case InitializeAction.Initialize:
                    InitializeAction initializeAction = (InitializeAction)action;
                    var actions = Task.Run(() => InitializeState(initializeAction.UserId));
                    actions.Wait();
                    break;
                case SetActivePanelAction.SetActivePanel:
                    SetActivePanelAction setActivePanelAction = (SetActivePanelAction)action;
                    SetActivePanel(setActivePanelAction.ActivePanel);
                    break;
                case ActivateLoadingAction.ActivateLoading:
                    ActivateLoadingAction activateLoadingAction = (ActivateLoadingAction)action;
                    ActivateLoading(activateLoadingAction.LoadingMessage);
                    break;
                case DeactivateLoadingAction.DeactivateLoading:
                    DeactivateLoading();
                    break;
                default:
                    break;
            }
        }

        public void SetActivePanel(AdminAppPanels panel) => _state.ActivePanel = panel;

        public async Task InitializeState(string activeUserId)
        {
            var newState = new ApplicationState();

            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            newState.ActiveUserId = activeUserId;

            newState.User.User = await userManager.FindByIdAsync(newState.ActiveUserId);
            newState.User.Claims = await userManager.GetClaimsAsync(newState.User.User) as List<Claim>;
            newState.User.Roles = await userManager.GetRolesAsync(newState.User.User) as List<string>;
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




    }
}
