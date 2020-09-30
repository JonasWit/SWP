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
        private readonly IServiceProvider serviceProvider;

        private GetClients GetClients => serviceProvider.GetService<GetClients>();
        private DeleteClient DeleteClient => serviceProvider.GetService<DeleteClient>();

        public LegalBlazorApp App { get; private set; }
        public List<ClientViewModel> ArchivizedClients { get; set; }
        public ClientViewModel SelectedArchivizedClient { get; set; }

        public ArchivePage(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public void RefreshData()
        {
            ArchivizedClients = GetClients.GetClientsWithoutData(App.User.Profile, false)?.Select(x => (ClientViewModel)x).ToList();
        }

        public void RefreshApp() => App.ForceRefresh();

        public void SelectedArchivizedClientChange(object client) => SelectedArchivizedClient = ArchivizedClients.FirstOrDefault(x => x.Id == int.Parse(client.ToString()));

        public async Task DeleteSelectedClient()
        {
            try
            {
                if (SelectedArchivizedClient != null)
                {
                    await DeleteClient.Delete(SelectedArchivizedClient.Id);
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
                App.ErrorPage.DisplayMessage(ex);
            }
        }
    }
}
