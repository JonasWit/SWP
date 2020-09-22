using Radzen;
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
        private readonly DeleteCashMovement deleteCashMovement;
        private readonly UpdateCashMovement updateCashMovement;

        public LegalBlazorApp App { get; private set; }
        public CreateCashMovement.Request NewCashMovement { get; set; } = new CreateCashMovement.Request();
        public int CashMovementDirection { get; set; }

        public FinancePage(
            CreateCashMovement createCashMovement,
            DeleteCashMovement deleteCashMovement,
            UpdateCashMovement updateCashMovement)
        {
            this.createCashMovement = createCashMovement;
            this.deleteCashMovement = deleteCashMovement;
            this.updateCashMovement = updateCashMovement;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public RadzenGrid<CashMovementViewModel> CashMovementGrid { get; set; }

        public void EditCashMovementRow(CashMovementViewModel cash) => CashMovementGrid.EditRow(cash);

        public async Task OnUpdateCashMovementRow(CashMovementViewModel cash)
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

        public void SaveCashMovementRow(CashMovementViewModel cash) => CashMovementGrid.UpdateRow(cash);

        public void CancelCashMovementEdit(CashMovementViewModel cash)
        {
            CashMovementGrid.CancelEditRow(cash);
            App.RefreshClients();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel cash)
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
            try
            {
                NewCashMovement.UpdatedBy = App.User.UserName;

                var result = await createCashMovement.Create(App.ActiveClient.Id, App.User.Profile, NewCashMovement);
                NewCashMovement = new CreateCashMovement.Request();

                App.ActiveClientWithData.CashMovements.Add(result);
                await CashMovementGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", App.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }







    }
}
