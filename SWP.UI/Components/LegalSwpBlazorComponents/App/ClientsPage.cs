using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ClientPage : BlazorPageBase
    {
        public enum Panels
        {
            Admin = 0,
            Jobs = 1,
            Main = 2
        }

        private DeleteClient DeleteClient => serviceProvider.GetService<DeleteClient>();
        private UpdateClient UpdateClient => serviceProvider.GetService<UpdateClient>();
        private CreateClient CreateClient => serviceProvider.GetService<CreateClient>();
        private CreateClientJob CreateClientJob => serviceProvider.GetService<CreateClientJob>();
        private DeleteClientJob DeleteClientJob => serviceProvider.GetService<DeleteClientJob>();
        private UpdateClientJob UpdateClientJob => serviceProvider.GetService<UpdateClientJob>();

        private readonly IServiceProvider serviceProvider;

        public LegalBlazorApp App { get; private set; }

        public ClientPage(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public Panels ActivePanel { get; set; }
        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        public CreateClient.Request NewClient { get; set; } = new CreateClient.Request();
        public CreateClientJob.Request NewClientJob { get; set; } = new CreateClientJob.Request();
        public RadzenGrid<ClientViewModel> ClientsGrid { get; set; }
        public RadzenGrid<ClientJobViewModel> ClientsJobsGrid { get; set; }

        #region Client

        public void EditClientRow(ClientViewModel client) => ClientsGrid.EditRow(client);

        public async Task OnUpdateClientRow(ClientViewModel client)
        {
            try
            {
                var result = await UpdateClient.Update(new UpdateClient.Request
                {
                    Id = client.Id,
                    Active = client.Active,
                    Address = client.Address,
                    Email = client.Email,
                    Name = client.Name,
                    PhoneNumber = client.PhoneNumber,
                    ProfileClaim = App.User.Profile,
                    UpdatedBy = App.User.UserName
                });

                if (App.ActiveClient != null && App.ActiveClient.Id == result.Id)
                {
                    App.ActiveClient = result;
                }
                else
                {
                    App.Clients[App.Clients.FindIndex(x => x.Id == result.Id)] = result;
                }

                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveClientRow(ClientViewModel client) => ClientsGrid.UpdateRow(client);

        public void CancelClientEdit(ClientViewModel client)
        {
            ClientsGrid.CancelEditRow(client);
            App.RefreshClients();
        }

        public async Task DeleteClientRow(ClientViewModel client)
        {
            try
            {
                await DeleteClient.Delete(client.Id);
                App.Clients.RemoveAll(x => x.Id == client.Id);

                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Klient: {client.Name} został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewClient(CreateClient.Request arg)
        {
            NewClient.ProfileClaim = App.User.Profile;
            NewClient.UpdatedBy = App.User.UserName;

            try
            {
                var result = await CreateClient.Do(NewClient);
                NewClient = new CreateClient.Request();

                App.Clients.Add(result);
                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion

        #region Jobs

        public async Task SubmitNewClientJob(CreateClientJob.Request arg)
        {
            try
            {
                NewClientJob.ProfileClaim = App.User.Profile;
                NewClientJob.ClientId = App.ActiveClient.Id;
                NewClientJob.UpdatedBy = App.User.UserName;

                var result = await CreateClientJob.Create(NewClientJob);
                NewClientJob = new CreateClientJob.Request();

                if (App.ActiveClientWithData != null)
                {
                    App.ActiveClientWithData.Jobs.Add(result);
                }

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {App.ActiveClient.Name} zostało stworzone.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void EditClientJobRow(ClientJobViewModel clientJob) => ClientsJobsGrid.EditRow(clientJob);

        public async Task OnUpdateClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                var result = await UpdateClientJob.Update(new UpdateClientJob.Request
                {
                    Id = clientJob.Id,
                    Active = clientJob.Active,
                    Description = clientJob.Description,
                    Name = clientJob.Name,
                    Priority = clientJob.Priority,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.Jobs[App.ActiveClientWithData.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {App.ActiveClient.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveClientJobRow(ClientJobViewModel clientJob) => ClientsJobsGrid.UpdateRow(clientJob);

        public void CancelClientJobEdit(ClientJobViewModel clientJob)
        {
            ClientsJobsGrid.CancelEditRow(clientJob);
            App.RefreshClientWithData();
        }

        public async Task DeleteClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                await DeleteClientJob.Delete(clientJob.Id);

                if (App.ActiveClientWithData != null)
                {
                    App.ActiveClientWithData.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                }

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Zadanie: {clientJob.Name}, dla Klineta: {App.ActiveClient.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveJobChange(object value)
        {
            var input = (ClientJobViewModel)value;
            if (value != null)
            {
                App.ActiveClientWithData.SelectedJob = App.ActiveClientWithData.Jobs.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedJob = null;
            }
        }

        #endregion
    }
}
