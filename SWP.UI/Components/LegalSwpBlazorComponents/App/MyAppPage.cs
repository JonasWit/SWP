using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class MyAppPage : BlazorPageBase, IDisposable
    {
        private readonly GetClients getClients;
        private readonly GetCases getCases;

        public LegalBlazorApp App { get; private set; }

        public List<DataItem> ClientsCases { get; set; } = new List<DataItem>();
        public List<DataItem> UserActivity { get; set; } = new List<DataItem>();

        public ColorScheme ColorScheme { get; set; } = ColorScheme.Palette;

        public MyAppPage(
            GetClients getClients,
            GetCases getCases)
        {
            this.getClients = getClients;
            this.getCases = getCases;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            RefreshClientsData();
            SubscribeToEvents();
            return Task.CompletedTask;
        }

        private void SubscribeToEvents()
        {
            App.ActiveClientChanged += new EventHandler(ActiveClientHasChanged);
        }

        private void UnsubscribeEvents()
        {
            App.ActiveClientChanged -= new EventHandler(ActiveClientHasChanged);
        }

        private void ActiveClientHasChanged(object sender, EventArgs e)
        {
            RefreshClientsData();
        }

        private void RefreshClientsData()
        {
            if (App.ActiveClient != null)
            {



                RefreshFinanceData();
            }
            else
            else
            {
                RefreshClinetCases();
                RefreshFinanceData();
            }
        }

        private void RefreshClinetCases()
        {
            ClientsCases.Clear();

            foreach (var client in App.Clients)
            {
                ClientsCases.Add(new DataItem
                {
                    Category = client.Name,
                    Number = getClients.CountCasesPerClient(client.Id)
                });
            }
        }

        private void RefreshFinanceData()
        {





        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}
