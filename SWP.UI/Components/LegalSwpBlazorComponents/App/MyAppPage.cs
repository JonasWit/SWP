using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.CashMovements;
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
        private readonly GetCashMovements getCashMovements;

        public LegalBlazorApp App { get; private set; }

        public List<CategoryDataItem> ClientsCases { get; set; } = new List<CategoryDataItem>();

        public List<ClientData> RevenueData { get; set; } = new List<ClientData>();

        public List<ClientData> ProductivityData { get; set; } = new List<ClientData>();

        public ColorScheme ColorScheme { get; set; } = ColorScheme.Monochrome;

        public MyAppPage(
            GetClients getClients,
            GetCases getCases,
            GetCashMovements getCashMovements)
        {
            this.getClients = getClients;
            this.getCases = getCases;
            this.getCashMovements = getCashMovements;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            RefreshData();
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
            RefreshData();
        }

        public void RefreshData()
        {
            if (App.ActiveClient != null)
            {
                RefreshClientCases();
                RefreshSpecificFinanceData();
            }
            else
            {
                RefreshClientCases();
                RefreshAllFinanceData();
            }
        }

        private void RefreshClientCases()
        {
            ClientsCases.Clear();

            foreach (var client in App.Clients)
            {
                ClientsCases.Add(new CategoryDataItem
                {
                    Category = client.Name,
                    Number = getClients.CountCasesPerClient(client.Id)
                });
            }
        }

        private void RefreshAllFinanceData()
        {
            RevenueData.Clear();

            foreach (var client in App.Clients)
            {
                var dataSet = new ClientData
                {
                    Name = client.Name
                };

                var cashMovements = getCashMovements.Get(client.Id);

                for (int i = 0; i < 13; i++)
                {
                    var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                    dataSet.DataByDate.Add(new DateDataItem
                    {
                        Date = date,
                        Number = Math.Round(cashMovements.Where(x => x.Created <= date).Sum(x => x.Amount), 2),
                    });
                }

                RevenueData.Add(dataSet);
            }
        }

        private void RefreshSpecificFinanceData()
        {
            RevenueData.Clear();

            var dataSet = new ClientData
            {
                Name = App.ActiveClient.Name
            };

            var cashMovements = getCashMovements.Get(App.ActiveClient.Id);

            for (int i = 0; i < 13; i++)
            {
                var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                dataSet.DataByDate.Add(new DateDataItem
                {
                    Date = date,
                    Number = Math.Round(cashMovements.Where(x => x.Created <= date).Sum(x => x.Amount), 2),
                });
            }

            RevenueData.Add(dataSet);

        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}
