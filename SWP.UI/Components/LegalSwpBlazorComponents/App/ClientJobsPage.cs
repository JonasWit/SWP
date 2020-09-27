﻿using Radzen;
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
        private IServiceProvider serviceProvider;

        public LegalBlazorApp App { get; private set; }

        private CreateClientJob CreateClientJob => serviceProvider.GetService<CreateClientJob>();
        private DeleteClientJob DeleteClientJob => serviceProvider.GetService<DeleteClientJob>();
        private UpdateClientJob UpdateClientJob => serviceProvider.GetService<UpdateClientJob>();
        public CreateClientJob.Request NewClientJob { get; set; } = new CreateClientJob.Request();
        public RadzenGrid<ClientJobViewModel> ClientsJobsGrid { get; set; }

        public ClientJobsPage(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

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
    }
}
