using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.Log;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Error;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Main
{
    public class MainState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public string ActiveUserId { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();
        public ClientViewModel ActiveClient { get; set; }
        public LegalAppPanels ActivePanel { get; set; } = LegalAppPanels.MyApp;
    }

    public class MainStore : StoreBase
    {
        private readonly MainState _state;
        private readonly NotificationService _notificationService;
        private readonly ErrorStore _errorStore;

        public MainState GetState() => _state;

        public MainStore(IServiceProvider serviceProvider, NotificationService notificationService, ErrorStore errorStore) : base(serviceProvider)
        {
            _state = new MainState();
            _notificationService = notificationService;
            _errorStore = errorStore;
        }

        public async Task InitializeState(string userId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                _state.ActiveUserId = userId;

                _state.User.User = await userManager.FindByIdAsync(_state.ActiveUserId);
                _state.User.Claims = await userManager.GetClaimsAsync(_state.User.User) as List<Claim>;
                _state.User.Roles = await userManager.GetRolesAsync(_state.User.User) as List<string>;
                _state.User.RelatedUsers = await userManager.GetUsersForClaimAsync(_state.User.ProfileClaim);

                _state.Clients = getClients.GetClientsWithoutData(_state.User.Profile)?
                    .Select(x => (ClientViewModel)x).ToList();
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
            _state.Loading = false;
        }

        public async Task ActiveClientChange(object value)
        {
            if (_state.Loading)
            {
                return;
            }
            else
            {
                ActivateLoading("Wczytywanie Klienta...");
            }

            if (value != null)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                    _state.ActiveClient = getClient.Get(int.Parse(value.ToString()));

                    //FinancePage.GetDataForMonthFilter();
                    //ProductivityPage.GetDataForMonthFilter();

                    DeactivateLoading();
                    BroadcastStateChange();
                }
                catch (Exception ex)
                {
                    await ShowErrorPage(ex);
                }
                finally
                {
                    DeactivateLoading();
                    BroadcastStateChange();
                }
            }
            else
            {
                if (_state.ActivePanel != LegalAppPanels.Calendar)
                {
                    SetActivePanel(LegalAppPanels.MyApp);
                }

                _state.ActiveClient = null;

                DeactivateLoading();
                BroadcastStateChange();
            }
        }

        public async Task ShowErrorPage(Exception ex)
        {
            await _errorStore.SetException(ex, _state.ActiveUserId);
            _state.ActivePanel = LegalAppPanels.ErrorPage;
            BroadcastStateChange();
        }

        public void DismissErrorPage()
        {
            _state.ActivePanel = LegalAppPanels.MyApp;
            BroadcastStateChange();
        }


    }
}
