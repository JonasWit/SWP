using Microsoft.Extensions.DependencyInjection;
using Radzen;
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
        public List<ClientViewModel> ArchivizedClients { get; set; }
        public ClientViewModel SelectedArchivizedClient { get; set; }
    }

    [UIScopedService]
    public class ArchiveStore : StoreBase
    {
        private readonly ArchiveState _state;

        public ArchiveState GetState() => _state;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ArchiveStore(IServiceProvider serviceProvider, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, notificationService, dialogService)
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

            _state.ArchivizedClients = getClients.GetClientsWithoutData(MainStore.GetState().User.Profile, false)?.Select(x => (ClientViewModel)x).ToList();
        }

        public void SelectedArchivizedClientChange(object client) => _state.SelectedArchivizedClient = _state.ArchivizedClients.FirstOrDefault(x => x.Id == int.Parse(client.ToString()));

        public async Task DeleteSelectedClient()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClientService = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                if (_state.SelectedArchivizedClient != null)
                {
                    await deleteClientService.Delete(_state.SelectedArchivizedClient.Id);
                    ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Klient: {_state.SelectedArchivizedClient.Name} został usunięty.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedClient = null;
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex).GetAwaiter();
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

                    await archiveClient.RecoverClient(_state.SelectedArchivizedClient.Id, MainStore.GetState().User.UserName);

                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {_state.SelectedArchivizedClient.Name} został odzyskany.", GeneralViewModel.NotificationDuration);
                    _state.SelectedArchivizedClient = null;
                    MainStore.RefreshClients();
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();       
            }
        }



    }
}
