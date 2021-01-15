using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.ViewModels.LegalApp.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MyApp
{

    public class MyAppState
    {
        public List<CategoryDataItem> ClientsCases { get; set; } = new List<CategoryDataItem>();
        public List<ClientData> ProductivityData { get; set; } = new List<ClientData>();
        public double TotalBalance => ProductivityData.Count != 0 ? ProductivityData.Sum(x => x.DataByDate.Sum(y => y.Number)) : 0;
        public double TotalExpenses => ProductivityData.Count != 0 ? ProductivityData.Sum(x => x.DataByDate.Sum(y => y.Expenses)) : 0;
        public double TotalTimeSpent => ProductivityData.Count != 0 ? ProductivityData.Sum(x => x.DataByDate.Sum(y => y.TimeNumber)) : 0;
        public ColorScheme ColorScheme { get; set; } = ColorScheme.Pastel;
    }

    [UIScopedService]
    public class MyAppStore : StoreBase<MyAppState>
    {
        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public MyAppStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
        }

        public Task Initialize()
        {
            RefreshSore();
            return Task.CompletedTask;
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                default:
                    break;
            }
        }

        private void RefreshClientCases()
        {
            _state.ClientsCases.Clear();

            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

            foreach (var client in MainStore.GetState().Clients)
            {
                _state.ClientsCases.Add(new CategoryDataItem
                {
                    Category = client.Name,
                    Number = getClients.CountCasesPerClient(client.Id)
                });
            }
        }

        private void RefreshSpecificProductivityData()
        {
            _state.ProductivityData.Clear();

            using var scope = _serviceProvider.CreateScope();
            var getTimeRecords = scope.ServiceProvider.GetRequiredService<GetTimeRecords>();
            var getCashMovements = scope.ServiceProvider.GetRequiredService<GetCashMovements>();

            var dataSet = new ClientData
            {
                Name = MainStore.GetState().ActiveClient.Name
            };

            var timeRecords = getTimeRecords.Get(MainStore.GetState().ActiveClient.Id);
            var cashMovements = getCashMovements.Get(MainStore.GetState().ActiveClient.Id);

            for (int i = 0; i < 13; i++)
            {
                var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                var records = timeRecords.Where(x => x.EventDate <= date).ToList();

                dataSet.DataByDate.Add(new DateDataItem
                {
                    Date = date,
                    Time = new TimeSpan(timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Hours), timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Minutes), 0),
                    Number = Math.Round(cashMovements.Where(x => x.EventDate <= date && !x.Expense).Sum(x => x.Amount), 2),
                    Expenses = Math.Abs(Math.Round(cashMovements.Where(x => x.EventDate <= date && x.Expense).Sum(x => x.Amount), 2)),
                });
            }

            _state.ProductivityData.Add(dataSet);
        }

        private void RefreshAllProductivityData()
        {
            _state.ProductivityData.Clear();

            using var scope = _serviceProvider.CreateScope();
            var getTimeRecords = scope.ServiceProvider.GetRequiredService<GetTimeRecords>();
            var getCashMovements = scope.ServiceProvider.GetRequiredService<GetCashMovements>();

            foreach (var client in MainStore.GetState().Clients)
            {
                var dataSet = new ClientData
                {
                    Name = client.Name
                };

                var timeRecords = getTimeRecords.Get(client.Id);
                var cashMovements = getCashMovements.Get(client.Id);

                for (int i = 0; i < 13; i++)
                {
                    var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                    var records = timeRecords.Where(x => x.EventDate <= date).ToList();

                    dataSet.DataByDate.Add(new DateDataItem
                    {
                        Date = date,
                        Time = new TimeSpan(timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Hours), timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Minutes), 0),
                        Number = Math.Round(cashMovements.Where(x => x.EventDate <= date && !x.Expense).Sum(x => x.Amount), 2),
                        Expenses = Math.Abs(Math.Round(cashMovements.Where(x => x.EventDate <= date && x.Expense).Sum(x => x.Amount), 2)),
                    });
                }

                _state.ProductivityData.Add(dataSet);
            }
        }

        public void RefreshSore()
        {
            Loading = true;

            if (MainStore.GetState().ActiveClient != null)
            {
                RefreshClientCases();
                RefreshSpecificProductivityData();
            }
            else
            {
                RefreshClientCases();
                RefreshAllProductivityData();
            }

            Loading = false;
        }
    }
}
