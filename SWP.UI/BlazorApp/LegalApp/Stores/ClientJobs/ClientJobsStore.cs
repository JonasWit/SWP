using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs
{
    public class ClientJobsState
    {
        public CreateClientJob.Request NewClientJob { get; set; } = new CreateClientJob.Request();
        public RadzenGrid<ClientJobViewModel> ClientsJobsGrid { get; set; }
        public ClientJobViewModel SelectedArchivizedClientJob { get; set; }
    }

    [UIScopedService]
    public class ClientJobsStore : StoreBase
    {
        private readonly ClientJobsState _state;

        public ClientJobsState GetState() => _state;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ClientJobsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _state = new ClientJobsState();
        }

        public void Initialize()
        {

        }

        public async Task SubmitNewClientJob(CreateClientJob.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createClientJob = scope.ServiceProvider.GetRequiredService<CreateClientJob>();

                request.ClientId = MainStore.GetState().ActiveClient.Id;
                request.UpdatedBy = MainStore.GetState().User.UserName;

                var result = await createClientJob.Create(request);
                _state.NewClientJob = new CreateClientJob.Request();

                if (MainStore.GetState().ActiveClient != null)
                {
                    MainStore.GetState().ActiveClient.Jobs.Add(result);
                }

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało stworzone.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void EditClientJobRow(ClientJobViewModel clientJob) => _state.ClientsJobsGrid.EditRow(clientJob);

        public async Task OnUpdateClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateClientJob = scope.ServiceProvider.GetRequiredService<UpdateClientJob>();

                var result = await updateClientJob.Update(new UpdateClientJob.Request
                {
                    Id = clientJob.Id,
                    Active = clientJob.Active,
                    Description = clientJob.Description,
                    Name = clientJob.Name,
                    Priority = clientJob.Priority,
                    Updated = DateTime.Now,
                    UpdatedBy = MainStore.GetState().User.UserName
                });

                MainStore.GetState().ActiveClient.Jobs[MainStore.GetState().ActiveClient.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SaveClientJobRow(ClientJobViewModel clientJob) => _state.ClientsJobsGrid.UpdateRow(clientJob);

        public void CancelClientJobEdit(ClientJobViewModel clientJob)
        {
            _state.ClientsJobsGrid.CancelEditRow(clientJob);
            MainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClientJob = scope.ServiceProvider.GetRequiredService<DeleteClientJob>();

                await deleteClientJob.Delete(clientJob.Id);

                if (MainStore.GetState().ActiveClient != null)
                {
                    MainStore.GetState().ActiveClient.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                }

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Zadanie: {clientJob.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void ActiveJobChange(object value)
        {
            var input = (ClientJobViewModel)value;
            if (value != null)
            {
                MainStore.GetState().ActiveClient.SelectedJob = MainStore.GetState().ActiveClient.Jobs.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                MainStore.GetState().ActiveClient.SelectedJob = null;
            }
        }

        public async Task ArchivizeClientJob(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                var result = await archiveJob.ArchivizeClientJob(clientJob.Id, MainStore.GetState().User.UserName);

                MainStore.GetState().ActiveClient.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                MainStore.GetState().ActiveClient.ArchivedJobs.Add(result);

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {clientJob.Name} zostało zarchwizowane.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SelectedArchivizedClientJobChange(object job) =>_state.SelectedArchivizedClientJob = MainStore.GetState().ActiveClient.ArchivedJobs.FirstOrDefault(x => x.Id == int.Parse(job.ToString()));

        public async Task RecoverSelectedJob()
        {
            try
            {
                if (_state.SelectedArchivizedClientJob != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                    var result = await archiveJob.RecoverClientJob(_state.SelectedArchivizedClientJob.Id, MainStore.GetState().User.UserName);

                    MainStore.GetState().ActiveClient.ArchivedJobs.RemoveAll(x => x.Id == _state.SelectedArchivizedClientJob.Id);
                    MainStore.GetState().ActiveClient.Jobs.Add(result);
                    _state.SelectedArchivizedClientJob = null;

                    await _state.ClientsJobsGrid.Reload();
                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name} zostało odzyskane.", GeneralViewModel.NotificationDuration);
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                await MainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        public override void CleanUpStore()
        {
            _state.SelectedArchivizedClientJob = null;
        }
    }
}
