using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive
{
    public class ArchiveState
    {
        public List<ClientViewModel> ArchivizedClients { get; set; } = new List<ClientViewModel>();
        public ClientViewModel SelectedArchivizedClient { get; set; }
        public List<CaseViewModel> ArchivizedCases { get; set; } = new List<CaseViewModel>();
        public CaseViewModel SelectedArchivizedCase { get; set; }
    }

    [UIScopedService]
    public class ArchiveStore : StoreBase
    {
        private readonly ArchiveState _state;

        public ArchiveState GetState() => _state;

        public MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ArchiveStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _state = new ArchiveState();
        }

        public void Initialize()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
            var getCases = scope.ServiceProvider.GetRequiredService<GetCases>();

            _state.ArchivizedClients = getClients.GetClientsWithoutData(_mainStore.GetState().User.Profile, false)?.Select(x => (ClientViewModel)x).ToList();

            if (_mainStore.GetState().ActiveClient != null)
            {
                _state.ArchivizedCases = getCases.GetArchivedCases(_mainStore.GetState().ActiveClient.Id).Select(x => (CaseViewModel)x).ToList();
            }
        }

        public void SelectedArchivizedClientChange(object client) => _state.SelectedArchivizedClient = _state.ArchivizedClients.FirstOrDefault(x => x.Id == int.Parse(client.ToString()));

        public void SelectedArchivizedCaseChange(object c) => _state.SelectedArchivizedCase = _state.ArchivizedCases.FirstOrDefault(x => x.Id == int.Parse(c.ToString()));

        public async Task DeleteSelectedClient()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClient = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                if (_state.SelectedArchivizedClient != null)
                {
                    await deleteClient.Delete(_state.SelectedArchivizedClient.Id);
                    ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Klient: {_state.SelectedArchivizedClient.Name} został usunięty.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedClient = null;
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Żaden klient nie został wybrany!", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
            finally 
            {
                BroadcastStateChange(); 
            }
        }

        public async Task RecoverSelectedClient()
        {
            try
            {
                if (_state.SelectedArchivizedClient != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveClient = scope.ServiceProvider.GetRequiredService<ArchiveClient>();

                    await archiveClient.RecoverClient(_state.SelectedArchivizedClient.Id, _mainStore.GetState().User.UserName);

                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {_state.SelectedArchivizedClient.Name} został odzyskany.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedClient = null;
                    _mainStore.RefreshClients();
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();       
            }
        }

        public async Task DeleteSelectedCase()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteCase = scope.ServiceProvider.GetRequiredService<DeleteCase>();

                if (_state.SelectedArchivizedCase != null)
                {
                    await deleteCase.Delete(_state.SelectedArchivizedCase.Id);
                    ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Sprawa: {_state.SelectedArchivizedCase.Name} została usunięta.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedCase = null;
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Żadna sprawa nie została wybrana!", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();
            }

        }

        public async Task RecoverSelectedCase()
        {
            try
            {
                if (_state.SelectedArchivizedCase != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveCases = scope.ServiceProvider.GetRequiredService<ArchiveCases>();

                    await archiveCases.RecoverCase(_state.SelectedArchivizedCase.Id, _mainStore.GetState().User.UserName);

                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {_state.SelectedArchivizedCase.Name} została odzyskana.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedClient = null;
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Żadna sprawa nie została wybrana!", GeneralViewModel.NotificationDuration);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        public override void CleanUpStore()
        {

        }
    }
}
