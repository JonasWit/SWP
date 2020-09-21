using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Threading.Tasks;

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

        private readonly DeleteClient deleteClient;
        private readonly UpdateClient updateClient;
        private readonly CreateClient createClient;
        private readonly CreateClientJob createClientJob;
        private readonly DeleteClientJob deleteClientJob;
        private readonly UpdateClientJob updateClientJob;
        private readonly GetClients getClients;

        public LegalBlazorApp App { get; private set; }

        public ClientPage(
            DeleteClient deleteClient,
            UpdateClient updateClient,
            CreateClient createClient,
            CreateClientJob createClientJob,
            DeleteClientJob deleteClientJob,
            UpdateClientJob updateClientJob,
            GetClients getClients)
        {
            this.deleteClient = deleteClient;
            this.updateClient = updateClient;
            this.createClient = createClient;
            this.createClientJob = createClientJob;
            this.deleteClientJob = deleteClientJob;
            this.updateClientJob = updateClientJob;
            this.getClients = getClients;
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

        public void EditClientRow(ClientViewModel Client) => ClientsGrid.EditRow(Client);

        public async Task OnUpdateClientRow(ClientViewModel Client)
        {
            try
            {
                var result = await updateClient.Update(new UpdateClient.Request
                {
                    Id = Client.Id,
                    Active = Client.Active,
                    Address = Client.Address,
                    Email = Client.Email,
                    Name = Client.Name,
                    PhoneNumber = Client.PhoneNumber,
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
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Client: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveClientRow(ClientViewModel Client) => ClientsGrid.UpdateRow(Client);

        public void CancelClientEdit(ClientViewModel Client)
        {
            ClientsGrid.CancelEditRow(Client);
            App.RefreshClients();
        }

        public async Task DeleteClientRow(ClientViewModel Client)
        {
            try
            {
                await deleteClient.Delete(Client.Id);
                App.Clients.RemoveAll(x => x.Id == Client.Id);

                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Client: {Client.Name} has been deleted.", 2000);
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
                var result = await createClient.Do(NewClient);
                NewClient = new CreateClient.Request();

                App.Clients.Add(result);
                await ClientsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Client: {result.Name} has been added.", 2000);
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

                var result = await createClientJob.Create(NewClientJob);
                NewClientJob = new CreateClientJob.Request();

                if (App.ActiveClientWithData != null)
                {
                    App.ActiveClientWithData.Jobs.Add(result);
                }

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Task: {result.Name} has been created.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
            finally
            {

            }
        }

        public void EditClientJobRow(ClientJobViewModel ClientJob) => ClientsJobsGrid.EditRow(ClientJob);

        public async Task OnUpdateClientJobRow(ClientJobViewModel ClientJob)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                var result = await updateClientJob.Update(new UpdateClientJob.Request
                {
                    Id = ClientJob.Id,
                    Active = ClientJob.Active,
                    Description = ClientJob.Description,
                    Name = ClientJob.Name,
                    Priority = ClientJob.Priority,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.Jobs[App.ActiveClientWithData.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Task: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                App.Loading = false;
            }
        }

        public void SaveClientJobRow(ClientJobViewModel ClientJob) => ClientsJobsGrid.UpdateRow(ClientJob);

        public void CancelClientJobEdit(ClientJobViewModel ClientJob)
        {
            ClientsJobsGrid.CancelEditRow(ClientJob);
            App.RefreshClients();
        }

        public async Task DeleteClientJobRow(ClientJobViewModel ClientJob)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                await deleteClientJob.Delete(ClientJob.Id);

                if (App.ActiveClientWithData != null)
                {
                    App.ActiveClientWithData.Jobs.RemoveAll(x => x.Id == ClientJob.Id);
                }

                await ClientsJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Task: {ClientJob.Name} has been deleted.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                App.Loading = false;
            }


        }

        #endregion
    }
}
