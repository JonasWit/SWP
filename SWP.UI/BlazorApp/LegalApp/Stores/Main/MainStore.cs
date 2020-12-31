using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Error;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Models;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Main
{
    public class MainState
    {
        public string ActiveUserId { get; set; }
        public AppActiveUserManager AppActiveUserManager { get; set; }
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();
        public ClientViewModel ActiveClient { get; set; }
        public LegalAppPanels ActivePanel { get; set; } = LegalAppPanels.MyApp;
        public string SelectedClientString { get; set; }
        public int UpcomingReminders { get; set; }
    }

    [UIScopedService]
    public class MainStore : StoreBase<MainState>
    {
        private readonly ErrorStore _errorStore;
        private readonly ILogger<MainStore> _logger;

        public MainStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, ErrorStore errorStore, ILogger<MainStore> logger)
            : base(serviceProvider, actionDispatcher, notificationService)
        {
            _errorStore = errorStore;
            _logger = logger;
        }

        public async Task Initialize(string userId)
        {
            try
            {
                _state.ActiveUserId = userId;
                await RealodUserData();
                ReloadClientsDrop();
                ReleadRemindersCounter();

                _logger.LogInformation(LogTags.LegalAppLogPrefix + "Legal Application accessed by user {userName}, with Profile {userProfile}", _state.AppActiveUserManager.UserName, _state.AppActiveUserManager.ProfileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogTags.LegalAppErrorLogPrefix + "Legal Application thrown an exception during launch for user {userName}, with Profile {userProfile}", _state.AppActiveUserManager.UserName, _state.AppActiveUserManager.ProfileName);
            }
        }

        public async Task RealodUserData()
        {
            if (!string.IsNullOrEmpty(_state.ActiveUserId))
            {
                _state.AppActiveUserManager = new AppActiveUserManager(_serviceProvider, _state.ActiveUserId);
                await _state.AppActiveUserManager.UpdateUserManager();
            }
        }

        protected override void HandleActions(IAction action)
        {

        }

        private void ReloadClientsDrop()
        {
            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

            //todo: add condition for profile & accesses
            _state.Clients = getClients.GetClientsWithoutData(_state.AppActiveUserManager.ProfileName)?.Select(x => (ClientViewModel)x).ToList();
        }

        private void ReleadRemindersCounter()
        {
            if (_state.AppActiveUserManager.IsRoot)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getReminders = scope.ServiceProvider.GetRequiredService<GetReminders>();

                    _state.UpcomingReminders = getReminders.GetUpcoming(_state.AppActiveUserManager.ProfileName, DateTime.Now.AddDays(2)).Count();
                }
                catch (Exception ex)
                {
                    ShowErrorPage(ex);
                }
            }   
        }

        public void SetActivePanel(LegalAppPanels panel) => _state.ActivePanel = panel;

        public void ActiveClientChange(object value)
        {
            try
            {
                if (value != null)
                {
                    var newId = int.Parse(value.ToString());

                    _state.ActiveClient = _state.Clients.FirstOrDefault(x => x.Id == newId);
                }
                else
                {
                    if (_state.ActivePanel != LegalAppPanels.Calendar)
                    {
                        SetActivePanel(LegalAppPanels.MyApp);
                    }

                    _state.ActiveClient = null;
                }
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void ShowErrorPage(Exception ex)
        {
            _errorStore.SetException(ex, _state.AppActiveUserManager.User.Id, _state.AppActiveUserManager.UserName);
            _state.ActivePanel = LegalAppPanels.ErrorPage;
            BroadcastStateChange();
        }

        public void DismissErrorPage()
        {
            _state.ActivePanel = LegalAppPanels.MyApp;
            _state.ActiveClient = null;
            _state.SelectedClientString = null;
            ReloadClientsDrop();
            BroadcastStateChange();
        }

        public async Task RefreshRelatedUsers()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                _state.AppActiveUserManager.RelatedUsers = await userManager.GetUsersForClaimAsync(_state.AppActiveUserManager.ProfileClaim) as List<IdentityUser>;
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
        }

        public void UpdateClientsList(ClientViewModel input) => _state.Clients[_state.Clients.FindIndex(x => x.Id == input.Id)] = input;

        public void RefreshClients()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

                _state.Clients = getClients.GetClientsWithoutData(_state.AppActiveUserManager.ProfileName, true).Select(x => (ClientViewModel)x).ToList();
                _state.ActiveClient = null;
                _state.SelectedClientString = null;
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
        }

        public void AddClient(ClientViewModel input) => _state.Clients.Add(input);

        public void RemoveClient(int id)
        {
            _state.Clients.RemoveAll(x => x.Id == id);

            if (_state.ActiveClient?.Id == id)
            {
                _state.ActiveClient = null;
                _state.SelectedClientString = null;
            }

            BroadcastStateChange();
        }

        public ClientViewModel GetActiveClient() => _state.ActiveClient;

        public AppActiveUserManager GetApplicationUser() => _state.AppActiveUserManager;

        public void RefreshMainComponent() => BroadcastStateChange();

        public void RefreshActiveClientData()
        {
            try
            {
                if (_state.ActiveClient != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                    ClientViewModel newModel = getClient.Get(_state.ActiveClient.Id);

                    _state.ActiveClient = newModel;
                }

                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
        }

        public override void CleanUpStore()
        {
            _state.SelectedClientString = null;
        }

        public override void RefreshSore()
        {
            ReloadClientsDrop();

            if (!_state.Clients.Any(x => x.Id != _state.ActiveClient.Id))
            {
                _state.SelectedClientString = null;
            }

            ReleadRemindersCounter();
            BroadcastStateChange();
        }
    }
}
