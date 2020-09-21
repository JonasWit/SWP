using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class FinancePage : BlazorPageBase
    {
        private readonly CreateCashMovement createCashMovement;

        public LegalBlazorApp App { get; private set; }

        public FinancePage(
            CreateCashMovement createCashMovement)
        {
            this.createCashMovement = createCashMovement;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public RadzenGrid<CashMovementViewModel> CashMovementGrid { get; set; }


        public void EditCashMovementRow(CashMovementViewModel Client) => CashMovementGrid.EditRow(Client);

        public async Task OnUpdateCashMovementRow(CashMovementViewModel Client)
        {
            try
            {
                //var result = await updateClient.Update(new UpdateClient.Request
                //{
                //    Id = Client.Id,
                //    Active = Client.Active,
                //    Address = Client.Address,
                //    Email = Client.Email,
                //    Name = Client.Name,
                //    PhoneNumber = Client.PhoneNumber,
                //    ProfileClaim = App.User.Profile,
                //    UpdatedBy = App.User.UserName
                //});

                //if (App.ActiveClient != null && App.ActiveClient.Id == result.Id)
                //{
                //    App.ActiveClient = result;
                //}
                //else
                //{
                //    App.Clients[App.Clients.FindIndex(x => x.Id == result.Id)] = result;
                //}

                //await ClientsGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Success!", $"Client: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveCashMovementRow(CashMovementViewModel Client) => CashMovementGrid.UpdateRow(Client);

        public void CancelCashMovementEdit(CashMovementViewModel Client)
        {
            //ClientsGrid.CancelEditRow(Client);
            //App.RefreshClients();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel Client)
        {
            try
            {
                //await deleteClient.Delete(Client.Id);
                //App.Clients.RemoveAll(x => x.Id == Client.Id);

                //await ClientsGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Client: {Client.Name} has been deleted.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request arg)
        {
            //NewClient.ProfileClaim = App.User.Profile;
            //NewClient.UpdatedBy = App.User.UserName;

            try
            {
                //var result = await createClient.Do(NewClient);
                //NewClient = new CreateClient.Request();

                //App.Clients.Add(result);
                //await ClientsGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Success!", $"Client: {result.Name} has been added.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }






    }
}
