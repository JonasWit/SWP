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
        public ClientJobViewModel SelectedJob { get; set; }
        public List<ClientJobViewModel> Jobs { get; set; } = new List<ClientJobViewModel>();
        public List<ClientJobViewModel> ArchivedJobs { get; set; } = new List<ClientJobViewModel>();
    }

    [UIScopedService]
    public class ClientJobsStore : StoreBase
    {
        private readonly ClientJobsState _state;

        public ClientJobsState GetState() => _state;

        public MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ClientJobsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _state = new ClientJobsState();
        }

        public void Initialize()
        {
            GetCashMovements(_mainStore.GetState().ActiveClient.Id);
        }

        public void GetCashMovements(int clientId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getJobs = scope.ServiceProvider.GetRequiredService<GetJobs>();

                var jobs = getJobs.GetClientJobs(clientId);

                _state.Jobs = jobs.Where(x => x.Active).Select(x => (ClientJobViewModel)x).ToList();
                _state.ArchivedJobs = jobs.Where(x => !x.Active).Select(x => (ClientJobViewModel)x).ToList();
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
        }

        public async Task SubmitNewClientJob(CreateClientJob.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createClientJob = scope.ServiceProvider.GetRequiredService<CreateClientJob>();

                request.ClientId = _mainStore.GetState().ActiveClient.Id;
                request.UpdatedBy = _mainStore.GetState().User.UserName;

                var result = await createClientJob.Create(request);
                _state.NewClientJob = new CreateClientJob.Request();

                _state.Jobs.Add(result);
                
                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {_mainStore.GetState().ActiveClient.Name} zostało stworzone.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
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
                    UpdatedBy = _mainStore.GetState().User.UserName
                });

                _state.Jobs[_state.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {_mainStore.GetState().ActiveClient.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SaveClientJobRow(ClientJobViewModel clientJob) => _state.ClientsJobsGrid.UpdateRow(clientJob);

        public void CancelClientJobEdit(ClientJobViewModel clientJob)
        {
            _state.ClientsJobsGrid.CancelEditRow(clientJob);
            _mainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClientJob = scope.ServiceProvider.GetRequiredService<DeleteClientJob>();

                await deleteClientJob.Delete(clientJob.Id);

                _state.Jobs.RemoveAll(x => x.Id == clientJob.Id);
               
                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Zadanie: {clientJob.Name}, dla Klineta: {_mainStore.GetState().ActiveClient.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void ActiveJobChange(object value)
        {
            var input = (ClientJobViewModel)value;
            if (value != null)
            {
                _state.SelectedJob = _state.Jobs.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedJob = null;
            }
        }

        public async Task ArchivizeClientJob(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                var result = await archiveJob.ArchivizeClientJob(clientJob.Id, _mainStore.GetState().User.UserName);

                _state.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                _state.ArchivedJobs.Add(result);

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {clientJob.Name} zostało zarchwizowane.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SelectedArchivizedClientJobChange(object job) =>_state.SelectedArchivizedClientJob = _state.ArchivedJobs.FirstOrDefault(x => x.Id == int.Parse(job.ToString()));

        public async Task RecoverSelectedJob()
        {
            try
            {
                if (_state.SelectedArchivizedClientJob != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                    var result = await archiveJob.RecoverClientJob(_state.SelectedArchivizedClientJob.Id, _mainStore.GetState().User.UserName);

                    _state.ArchivedJobs.RemoveAll(x => x.Id == _state.SelectedArchivizedClientJob.Id);
                    _state.Jobs.Add(result);
                    _state.SelectedArchivizedClientJob = null;

                    await _state.ClientsJobsGrid.Reload();
                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name} zostało odzyskane.", GeneralViewModel.NotificationDuration);
                }
                else
                {
                    ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                }

                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        protected override void HandleActions(IAction action)
        {

        }

        public override void CleanUpStore()
        {
            _state.SelectedArchivizedClientJob = null;
            _state.SelectedJob = null;
        }

        public override void RefreshSore()
        {
            GetCashMovements(_mainStore.GetState().ActiveClient.Id);
        }
    }
}
