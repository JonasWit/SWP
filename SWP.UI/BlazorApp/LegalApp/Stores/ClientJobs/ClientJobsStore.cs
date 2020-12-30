using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions;
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
    public class ClientJobsStore : StoreBase<ClientJobsState>
    {
        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public ClientJobsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {

        }

        public void Initialize()
        {
            GetClientJobs(MainStore.GetState().ActiveClient.Id);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SubmitNewClientJobAction.SubmitNewClientJob:
                    var submitNewClientJobAction = (SubmitNewClientJobAction)action;
                    await SubmitNewClientJob(submitNewClientJobAction.Arg);
                    break;
                case EditClientJobRowAction.EditClientJobRow:
                    var editClientJobRowAction = (EditClientJobRowAction)action;
                    EditClientJobRow(editClientJobRowAction.Arg);
                    break;
                case OnUpdateClientJobRowAction.OnUpdateClientJobRow:
                    var onUpdateClientJobRowAction = (OnUpdateClientJobRowAction)action;
                    await OnUpdateClientJobRow(onUpdateClientJobRowAction.Arg);
                    break;
                case SaveClientJobRowAction.SaveClientJobRow:
                    var saveClientJobRowAction = (SaveClientJobRowAction)action;
                    SaveClientJobRow(saveClientJobRowAction.Arg);
                    break;
                case CancelClientJobEditAction.CancelClientJobEdit:
                    var cancelClientJobEditAction = (CancelClientJobEditAction)action;
                    CancelClientJobEdit(cancelClientJobEditAction.Arg);
                    break;
                case DeleteClientJobRowAction.DeleteClientJobRow:
                    var deleteClientJobRowAction = (DeleteClientJobRowAction)action;
                    await DeleteClientJobRow(deleteClientJobRowAction.Arg);
                    break;
                case ActiveJobChangeAction.ActiveJobChange:
                    var activeJobChangeAction = (ActiveJobChangeAction)action;
                    ActiveJobChange(activeJobChangeAction.Arg);
                    break;
                case ArchivizeClientJobAction.ArchivizeClientJob:
                    var archivizeClientJobAction = (ArchivizeClientJobAction)action;
                    await ArchivizeClientJob(archivizeClientJobAction.Arg);
                    break;
                case SelectedArchivizedClientJobChangeAction.SelectedArchivizedClientJobChange:
                    var selectedArchivizedClientJobChangeAction = (SelectedArchivizedClientJobChangeAction)action;
                    SelectedArchivizedClientJobChange(selectedArchivizedClientJobChangeAction.Arg);
                    break;
                case RecoverSelectedJobAction.RecoverSelectedJob:
                    await RecoverSelectedJob();
                    break;
                default:
                    break;
            }
        }

        private void GetClientJobs(int clientId)
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
                MainStore.ShowErrorPage(ex);
            }
        }

        #region Actions

        private async Task SubmitNewClientJob(CreateClientJob.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createClientJob = scope.ServiceProvider.GetRequiredService<CreateClientJob>();

                request.ClientId = MainStore.GetState().ActiveClient.Id;
                request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

                var result = await createClientJob.Create(request);
                _state.NewClientJob = new CreateClientJob.Request();

                _state.Jobs.Add(result);

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało stworzone.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void EditClientJobRow(ClientJobViewModel clientJob) => _state.ClientsJobsGrid.EditRow(clientJob);

        private async Task OnUpdateClientJobRow(ClientJobViewModel clientJob)
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
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                _state.Jobs[_state.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveClientJobRow(ClientJobViewModel clientJob) => _state.ClientsJobsGrid.UpdateRow(clientJob);

        private void CancelClientJobEdit(ClientJobViewModel clientJob)
        {
            _state.ClientsJobsGrid.CancelEditRow(clientJob);
            MainStore.RefreshMainComponent();
            BroadcastStateChange();
        }

        private async Task DeleteClientJobRow(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClientJob = scope.ServiceProvider.GetRequiredService<DeleteClientJob>();

                await deleteClientJob.Delete(clientJob.Id);

                _state.Jobs.RemoveAll(x => x.Id == clientJob.Id);

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Zadanie: {clientJob.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ActiveJobChange(object value)
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

        private async Task ArchivizeClientJob(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                var result = await archiveJob.ArchivizeClientJob(clientJob.Id, MainStore.GetState().AppActiveUserManager.UserName);

                _state.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                _state.ArchivedJobs.Add(result);

                await _state.ClientsJobsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {clientJob.Name} zostało zarchwizowane.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SelectedArchivizedClientJobChange(object job) => _state.SelectedArchivizedClientJob = _state.ArchivedJobs.FirstOrDefault(x => x.Id == int.Parse(job.ToString()));

        private async Task RecoverSelectedJob()
        {
            try
            {
                if (_state.SelectedArchivizedClientJob != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                    var result = await archiveJob.RecoverClientJob(_state.SelectedArchivizedClientJob.Id, MainStore.GetState().AppActiveUserManager.UserName);

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
                MainStore.ShowErrorPage(ex);
            }
        }

        #endregion

        public override void CleanUpStore()
        {
            _state.SelectedArchivizedClientJob = null;
            _state.SelectedJob = null;
        }

        public override void RefreshSore()
        {
            GetClientJobs(MainStore.GetState().ActiveClient.Id);
        }
    }
}
