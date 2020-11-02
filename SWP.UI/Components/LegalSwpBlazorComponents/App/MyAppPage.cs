using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.Dialogs;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
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
        private readonly DialogService _dialogService;
        private readonly GeneralViewModel _generalViewModel;

        public LegalBlazorApp App { get; private set; }
        public List<CategoryDataItem> ClientsCases { get; set; } = new List<CategoryDataItem>();
        public List<ClientData> ProductivityData { get; set; } = new List<ClientData>();
        public double TotalBalance => ProductivityData.Count != 0 ? ProductivityData.Sum(x => x.DataByDate.Sum(y => y.Number)) : 0;
        public IdentityUser SelectedUser { get; set; }
        public ColorScheme ColorScheme { get; set; } = ColorScheme.Pastel;

        public MyAppPage(IServiceProvider serviceProvider, DialogService dialogService, GeneralViewModel generalViewModel) : base(serviceProvider) 
        {
            _dialogService = dialogService;
            _generalViewModel = generalViewModel;
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

            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

            foreach (var client in App.Clients)
            {
                ClientsCases.Add(new CategoryDataItem
                {
                    Category = client.Name,
                    Number = getClients.CountCasesPerClient(client.Id)
                });
            }
        }

        private void RefreshSpecificProductivityData()
        {
            ProductivityData.Clear();

            using var scope = _serviceProvider.CreateScope();
            var getTimeRecords = scope.ServiceProvider.GetRequiredService<GetTimeRecords>();
            var getCashMovements = scope.ServiceProvider.GetRequiredService<GetCashMovements>();

            var dataSet = new ClientData
            {
                Name = App.ActiveClient.Name
            };

            var timeRecords = getTimeRecords.Get(App.ActiveClient.Id);
            var cashMovements = getCashMovements.Get(App.ActiveClient.Id);

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

            ProductivityData.Add(dataSet);
        }

        private void RefreshAllProductivityData()
        {
            ProductivityData.Clear();

            using var scope = _serviceProvider.CreateScope();
            var getTimeRecords = scope.ServiceProvider.GetRequiredService<GetTimeRecords>();
            var getCashMovements = scope.ServiceProvider.GetRequiredService<GetCashMovements>();

            foreach (var client in App.Clients)
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
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var profileClaim = App.User.ProfileClaim;
                var selectedUser = App.User.RelatedUsers.FirstOrDefault(x => x.Id == SelectedUser.Id);
                var result = await userManager.RemoveClaimAsync(selectedUser, profileClaim);

                SelectedUser = App.User.User;
                await App.RefreshRelatedUsers();
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public async Task RemoveProfileData()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClient = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                await deleteClient.Delete(App.User.Profile);

                App.RefreshClients();
                App.ForceRefresh();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Usunięto wszystkie dane powiązane z profilem: {App.User.Profile}", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public void ConfirmRemoveRelation()
        {
            _dialogService.Open<GenericDialogPopup>("Order TEST",
                new Dictionary<string, object>()
                {
                    { "Title", "TEST Title Passed" },
                    { "TaskToExecuteAsync", new Func<Task>(App.ErrorPage.ThrowTestException) },
                    { "Description", "This is sample Description" },
                },
                _generalViewModel.DefaultDialogOptions);
        }

        #endregion
    }
}
