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
        public LegalAppPanels ActivePanel { get; set; } = LegalAppPanels.MyApp;
    }

    public class MainStore : StoreBase
    {
        private MainState _state;

        public MainState GetState() => _state;

        public MainStore(IServiceProvider serviceProvider) : base(serviceProvider)
        {

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

        public void SetActivePanel(LegalAppPanels panel) => _state.ActivePanel = panel;

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
