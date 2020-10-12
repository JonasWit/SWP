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
using SWP.Application.LegalSwp.ContactPeopleAdmin;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ClientPage : BlazorPageBase
    {
        private DeleteClient DeleteClient => serviceProvider.GetService<DeleteClient>();
        private UpdateClient UpdateClient => serviceProvider.GetService<UpdateClient>();
        private CreateClient CreateClient => serviceProvider.GetService<CreateClient>();
        private ArchiveClient ArchiveClient => serviceProvider.GetService<ArchiveClient>();
        public ClientViewModel SelectedClient { get; set; }
        public LegalBlazorApp App { get; private set; }

        public ClientPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public CreateClient.Request NewClient { get; set; } = new CreateClient.Request();
        public RadzenGrid<ClientViewModel> ClientsGrid { get; set; }

        public void ClientSelected(object value)
        {
            var input = (ClientViewModel)value;
            if (value != null)
            {
                SelectedClient = App.Clients.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                SelectedClient = null;
            }
        }

        #region Client

        private void RefreshSelectedClient() => SelectedClient = App.Clients.FirstOrDefault(x => x.Id == SelectedClient.Id);

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

                App.RefreshClients();
                await ClientsGrid.Reload();
                RefreshSelectedClient();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveClientRow(ClientViewModel client) => ClientsGrid.UpdateRow(client);

        public void CancelClientEdit(ClientViewModel client)
        {
            ClientsGrid.CancelEditRow(client);
            App.RefreshClients();
            RefreshSelectedClient();
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
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task ArchivizeClient(ClientViewModel client)
        {
            try
            {
                var result = await ArchiveClient.ArchivizeClient(client.Id, App.User.UserName);
                SelectedClient = null;

                if (App.ActiveClient != null && App.ActiveClient.Id == client.Id)
                {
                    App.ActiveClient = null;
                }
                else
                {
                    App.Clients.RemoveAll(x => x.Id == client.Id);
                }

                App.RefreshClients();
                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {client.Name} został zarchwizowany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewClient(CreateClient.Request request)
        {
            if (request == null) return;

            request.ProfileClaim = App.User.Profile;
            request.UpdatedBy = App.User.UserName;

            try
            {
                var result = await CreateClient.Do(request);
                NewClient = new CreateClient.Request();

                App.Clients.Add(result);
                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion

    }
}
