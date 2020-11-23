using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients
{
    public class ClientsState
    {
        public ClientViewModel SelectedClient { get; set; }
        public CreateClient.Request NewClient { get; set; } = new CreateClient.Request();
        public RadzenGrid<ClientViewModel> ClientsGrid { get; set; }
    }

    [UIScopedService]
    public class ClientsStore : StoreBase
    {
        private readonly ClientsState _state;

        public ClientsState GetState() => _state;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ClientsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService) : base(serviceProvider, actionDispatcher, notificationService)
        {
            _state = new ClientsState();
        }

        public void ClientSelected(object value)
        {
            var input = (ClientViewModel)value;
            if (value != null)
            {
                _state.SelectedClient = MainStore.GetState().Clients.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedClient = null;
            }
        }

        #region Client

        private void RefreshSelectedClient()
        {
            if (MainStore.GetState().Clients.Any(x => x.Id == _state.SelectedClient.Id))
            {
                _state.SelectedClient = MainStore.GetState().Clients.FirstOrDefault(x => x.Id == _state.SelectedClient.Id);
            }    
        }
          
        public void EditClientRow(ClientViewModel client) => _state.ClientsGrid.EditRow(client);

        public async Task OnUpdateClientRow(ClientViewModel client)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateClient = scope.ServiceProvider.GetRequiredService<UpdateClient>();

                var result = await updateClient.Update(new UpdateClient.Request
                {
                    Id = client.Id,
                    Active = client.Active,
                    Address = client.Address,
                    Email = client.Email,
                    Name = client.Name,
                    PhoneNumber = client.PhoneNumber,
                    UpdatedBy = MainStore.GetState().User.UserName
                });

                if (MainStore.GetState().ActiveClient != null && MainStore.GetState().ActiveClient.Id == result.Id)
                {
                    await MainStore.ActiveClientChange(result.Id);
                }
                else
                {
                    MainStore.UpdateClientsList(result);
                }

                MainStore.RefreshClients();
                await _state.ClientsGrid.Reload();

                RefreshSelectedClient();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został zmieniony.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
        }

        public void SaveClientRow(ClientViewModel client) => _state.ClientsGrid.UpdateRow(client);

        public void CancelClientEdit(ClientViewModel client)
        {
            _state.ClientsGrid.CancelEditRow(client);
            MainStore.RefreshClients();
            RefreshSelectedClient();
            BroadcastStateChange();
        }

        public async Task DeleteClientRow(ClientViewModel client)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClient = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                await deleteClient.Delete(client.Id);
                MainStore.RemoveClient(client.Id);

                if (_state.SelectedClient != null &&
                    _state.SelectedClient.Id == client.Id)
                {
                    _state.SelectedClient = null;
                }

                await _state.ClientsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Klient: {client.Name} został usunięty.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
        }

        public async Task ArchivizeClient(ClientViewModel client)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveClient = scope.ServiceProvider.GetRequiredService<ArchiveClient>();

                var result = await archiveClient.ArchivizeClient(client.Id, MainStore.GetState().User.UserName);

                if (_state.SelectedClient != null &&
                    _state.SelectedClient.Id == client.Id)
                {
                    _state.SelectedClient = null;
                }

                if (MainStore.GetState().ActiveClient != null && MainStore.GetState().ActiveClient.Id == client.Id)
                {
                    await MainStore.ActiveClientChange(null);
                }
                else
                {
                    MainStore.RemoveClient(client.Id);
                }

                MainStore.RefreshClients();
                await _state.ClientsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {client.Name} został zarchwizowany.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
        }

        public async Task SubmitNewClient(CreateClient.Request request)
        {
            if (request == null) return;

            request.ProfileClaim = MainStore.GetState().User.Profile;
            request.UpdatedBy = MainStore.GetState().User.UserName;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createClient = scope.ServiceProvider.GetRequiredService<CreateClient>();

                var result = await createClient.Do(request);
                _state.NewClient = new CreateClient.Request();

                MainStore.AddClient(result);

                if (_state.ClientsGrid != null)
                {
                    await _state.ClientsGrid.Reload();
                }
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Klient: {result.Name} został dodany.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        public override void CleanUpStore()
        {
            _state.SelectedClient = null;
        }

        public override void RefreshSore()
        {
    
        }

        #endregion





    }
}
