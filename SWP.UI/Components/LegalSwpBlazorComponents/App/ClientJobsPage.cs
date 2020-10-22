using Radzen;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Radzen.Blazor;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ClientJobsPage : BlazorPageBase
    {
        public LegalBlazorApp App { get; private set; }

        public GeneralViewModel _generalViewModel;
        public CreateClientJob.Request NewClientJob { get; set; } = new CreateClientJob.Request();
        public RadzenGrid<ClientJobViewModel> ClientsJobsGrid { get; set; }
        public ClientJobViewModel SelectedArchivizedClientJob { get; set; }

        public ClientJobsPage(IServiceProvider serviceProvider, GeneralViewModel generalViewModel) : base(serviceProvider) 
        {
            _generalViewModel = generalViewModel;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public async Task SubmitNewClientJob(CreateClientJob.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createClientJob = scope.ServiceProvider.GetRequiredService<CreateClientJob>();

                request.ClientId = App.ActiveClient.Id;
                request.UpdatedBy = App.User.UserName;

                var result = await createClientJob.Create(request);
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
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public void EditClientJobRow(ClientJobViewModel clientJob) => ClientsJobsGrid.EditRow(clientJob);

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
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.Jobs[App.ActiveClientWithData.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {App.ActiveClient.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
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
                using var scope = _serviceProvider.CreateScope();
                var deleteClientJob = scope.ServiceProvider.GetRequiredService<DeleteClientJob>();

                await deleteClientJob.Delete(clientJob.Id);

                if (App.ActiveClientWithData != null)
                {
                    App.ActiveClientWithData.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                }

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Zadanie: {clientJob.Name}, dla Klineta: {App.ActiveClient.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
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

        public async Task ArchivizeClientJob(ClientJobViewModel clientJob)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                var result = await archiveJob.ArchivizeClientJob(clientJob.Id, App.User.UserName);

                App.ActiveClientWithData.Jobs.RemoveAll(x => x.Id == clientJob.Id);
                App.ActiveClientWithData.ArchivedJobs.Add(result);

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {clientJob.Name} zostało zarchwizowane.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }

        public void SelectedArchivizedClientJobChange(object job) => SelectedArchivizedClientJob = App.ActiveClientWithData.ArchivedJobs.FirstOrDefault(x => x.Id == int.Parse(job.ToString()));

        public async Task RecoverSelectedJob()
        {
            try
            {
                if (SelectedArchivizedClientJob != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var archiveJob = scope.ServiceProvider.GetRequiredService<ArchiveJob>();

                    var result = await archiveJob.RecoverClientJob(SelectedArchivizedClientJob.Id, App.User.UserName);

                    App.ActiveClientWithData.ArchivedJobs.RemoveAll(x => x.Id == SelectedArchivizedClientJob.Id);
                    App.ActiveClientWithData.Jobs.Add(result);
                    SelectedArchivizedClientJob = null;

                    await ClientsJobsGrid.Reload();
                    App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name} zostało odzyskane.", GeneralViewModel.NotificationDuration);
                }
                else
                {
                    App.ShowNotification(NotificationSeverity.Error, "Uwaga!", $"Brak zaznaczonego klienta!", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessageAsync(ex);
            }
        }
    }
}
