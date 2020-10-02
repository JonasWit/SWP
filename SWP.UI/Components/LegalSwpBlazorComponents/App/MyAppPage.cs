using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.TimeRecords;
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
        private GetClients GetClients => serviceProvider.GetService<GetClients>();
        private GetCashMovements GetCashMovements => serviceProvider.GetService<GetCashMovements>();
        private GetTimeRecords GetTimeRecords => serviceProvider.GetService<GetTimeRecords>();
        private UserManager<IdentityUser> UserManager => serviceProvider.GetService<UserManager<IdentityUser>>();
        public LegalBlazorApp App { get; private set; }
        public List<CategoryDataItem> ClientsCases { get; set; } = new List<CategoryDataItem>();
        public List<ClientData> ProductivityData { get; set; } = new List<ClientData>();
        public IdentityUser SelectedUser { get; set; }
        public ColorScheme ColorScheme { get; set; } = ColorScheme.Monochrome;
        public MyAppPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

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
                RefreshSpecificProductivityData();
            }
            else
            {
                RefreshClientCases();
                RefreshAllProductivityData();
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
                    Number = GetClients.CountCasesPerClient(client.Id)
                });
            }
        }

        private void RefreshSpecificProductivityData()
        {
            ProductivityData.Clear();

            var dataSet = new ClientData
            {
                Name = App.ActiveClient.Name
            };

            var timeRecords = GetTimeRecords.Get(App.ActiveClient.Id);
            var cashMovements = GetCashMovements.Get(App.ActiveClient.Id);

            for (int i = 0; i < 13; i++)
            {
                var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                var records = timeRecords.Where(x => x.EventDate <= date).ToList();

                dataSet.DataByDate.Add(new DateDataItem
                {
                    Date = date,
                    Time = new TimeSpan(timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Hours), timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Minutes), 0),
                    Number = Math.Round(cashMovements.Where(x => x.EventDate <= date).Sum(x => x.Amount), 2),
                });
            }

            ProductivityData.Add(dataSet);
        }

        private void RefreshAllProductivityData()
        {
            ProductivityData.Clear();

            foreach (var client in App.Clients)
            {
                var dataSet = new ClientData
                {
                    Name = client.Name
                };

                var timeRecords = GetTimeRecords.Get(client.Id);
                var cashMovements = GetCashMovements.Get(client.Id);

                for (int i = 0; i < 13; i++)
                {
                    var date = new DateTime(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-i).Year, DateTime.Now.AddMonths(-i).Month));
                    var records = timeRecords.Where(x => x.EventDate <= date).ToList();

                    dataSet.DataByDate.Add(new DateDataItem
                    {
                        Date = date,
                        Time = new TimeSpan(timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Hours), timeRecords.Where(x => x.EventDate <= date).Sum(x => x.Minutes), 0),
                        Number = Math.Round(cashMovements.Where(x => x.EventDate <= date).Sum(x => x.Amount), 2),
                    });
                }

                ProductivityData.Add(dataSet);
            }
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        #region Users Features

        public void SelectedUserChange(object value)
        {
            var input = (string)value;
            if (value != null)
            {
                SelectedUser = App.User.RelatedUsers.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                SelectedUser = null;
            }
        }

        public async Task RemoveRelation()
        {
            try
            {
                var profileClaim = App.User.ProfileClaim;
                var selectedUser = App.User.RelatedUsers.FirstOrDefault(x => x.Id == SelectedUser.Id);
                var result = await UserManager.RemoveClaimAsync(selectedUser, profileClaim);

                SelectedUser = App.User.User;
                await App.RefreshRelatedUsers();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion
    }
}
