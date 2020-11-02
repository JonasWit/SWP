using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.Log;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.MainStore.Actions;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MainStore
{
    public class MainState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public string ActiveUserId { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();
        public ClientViewModel ActiveClientWithData { get; set; }
        public ClientViewModel ActiveClient { get; set; }
        public Panels ActivePanel { get; set; } = Panels.MyApp;
    }

    public class MainStore
    {
        private readonly IActionDispatcher _actionDispatcher;
        private readonly IServiceProvider _serviceProvider;
        private MainState _state;

        public MainStore(IActionDispatcher actionDispatcher, IServiceProvider serviceProvider)
        {
            _actionDispatcher = actionDispatcher;
            _serviceProvider = serviceProvider;
            _actionDispatcher.Subscribe(HandleActions);
        }

        public MainState GetState() => _state;

        private void HandleActions(IAction action)
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

        public async Task InitializeState(string userId)
        {
            try
            {
                var newState = new MainState();

                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                newState.ActiveUserId = userId;

                newState.User.User = await userManager.FindByIdAsync(newState.ActiveUserId);
                newState.User.Claims = await userManager.GetClaimsAsync(newState.User.User) as List<Claim>;
                newState.User.Roles = await userManager.GetRolesAsync(newState.User.User) as List<string>;
                newState.User.RelatedUsers = await userManager.GetUsersForClaimAsync(newState.User.ProfileClaim);

                newState.Clients = getClients.GetClientsWithoutData(newState.User.Profile)?
                    .Select(x => (ClientViewModel)x).ToList();

                _state = newState;
            }
            catch (Exception ex)
            {
                using var scope = _serviceProvider.CreateScope();
                var createLogRecord = scope.ServiceProvider.GetRequiredService<CreateLogRecord>();

                await createLogRecord.Create(new CreateLogRecord.Request
                {
                    Message = ex.Message,
                    UserId = _state.ActiveUserId,
                    StackTrace = ex.StackTrace
                });
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SetActivePanel(Panels panel) => _state.ActivePanel = panel;

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

        #region Observer pattern

        private Action _listeners;

        public void AddStateChangeListener(Action listener)
        {
            _listeners += listener;
        }

        public void RemoveStateChangeListener(Action listener)
        {
            _listeners -= listener;
        }

        private void BroadcastStateChange()
        {
            _listeners.Invoke();
        }

        #endregion

    }
}
