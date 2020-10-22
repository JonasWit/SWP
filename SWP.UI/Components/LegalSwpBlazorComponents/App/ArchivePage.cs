using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ArchivePage : BlazorPageBase
    {
        public LegalBlazorApp App { get; private set; }
        public List<ClientViewModel> ArchivizedClients { get; set; }
        public ClientViewModel SelectedArchivizedClient { get; set; }

        public ArchivePage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public void RefreshData()
        {
            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

            ArchivizedClients = getClients.GetClientsWithoutData(App.User.Profile, false)?.Select(x => (ClientViewModel)x).ToList();
        }

        public void RefreshApp() => App.ForceRefresh();

        public void SelectedArchivizedClientChange(object client) => SelectedArchivizedClient = ArchivizedClients.FirstOrDefault(x => x.Id == int.Parse(client.ToString()));

        public async Task DeleteSelectedClient()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClientService = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                if (SelectedArchivizedClient != null)
                {
                    await deleteClientService.Delete(SelectedArchivizedClient.Id);
                    App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Klient: {SelectedArchivizedClient.Name} został usunięty.", GeneralViewModel.NotificationDuration);
                    SelectedArchivizedClient = null;
                    RefreshData();
                }
                else
                {
                    App.ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public async Task RecoverSelectedClient()
        {
            try
            {
                if (SelectedArchivizedClient != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveClient = scope.ServiceProvider.GetRequiredService<ArchiveClient>();

                    await archiveClient.RecoverClient(SelectedArchivizedClient.Id, App.User.UserName);

                    App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {SelectedArchivizedClient.Name} został odzyskany.", GeneralViewModel.NotificationDuration);
                    SelectedArchivizedClient = null;
                    RefreshData();
                    App.RefreshClients();
                }
                else
                {
                    App.ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }
    }
}
