using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using Microsoft.AspNetCore.Server.HttpSys;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ArchivePage : BlazorPageBase
    {
        private readonly IServiceProvider serviceProvider;

        private GetClients GetClients => serviceProvider.GetService<GetClients>();

        public LegalBlazorApp App { get; private set; }
        public List<ClientViewModel> ArchivizedClients { get; set; }
        public ClientViewModel SelectedArchivizedClient { get; set; }

        public ArchivePage(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;


        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            RefreshData();
            return Task.CompletedTask;
        }

        private void RefreshData()
        {
            ArchivizedClients = GetClients.GetClientsWithoutData(App.User.Profile, false)?.Select(x => (ClientViewModel)x).ToList();
        }

        public void RefreshApp() => App.ForceRefresh();

        public void SelectedArchivizedClientChange(object client) => SelectedArchivizedClient = ArchivizedClients.FirstOrDefault(x => x.Id == int.Parse(client.ToString()));




    }
}
