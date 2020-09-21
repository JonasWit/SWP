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


        public void EditCashMovementRow(CashMovementViewModel customer) => CashMovementGrid.EditRow(customer);

        public async Task OnUpdateCashMovementRow(CashMovementViewModel customer)
        {
            try
            {
                //var result = await updateCustomer.Update(new UpdateCustomer.Request
                //{
                //    Id = customer.Id,
                //    Active = customer.Active,
                //    Address = customer.Address,
                //    Email = customer.Email,
                //    Name = customer.Name,
                //    PhoneNumber = customer.PhoneNumber,
                //    ProfileClaim = App.User.Profile,
                //    UpdatedBy = App.User.UserName
                //});

                //if (App.ActiveCustomer != null && App.ActiveCustomer.Id == result.Id)
                //{
                //    App.ActiveCustomer = result;
                //}
                //else
                //{
                //    App.Customers[App.Customers.FindIndex(x => x.Id == result.Id)] = result;
                //}

                //await CustomersGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Success!", $"Customer: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveCashMovementRow(CashMovementViewModel customer) => CashMovementGrid.UpdateRow(customer);

        public void CancelCashMovementEdit(CashMovementViewModel customer)
        {
            //CustomersGrid.CancelEditRow(customer);
            //App.RefreshCustomers();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel customer)
        {
            try
            {
                //await deleteCustomer.Delete(customer.Id);
                //App.Customers.RemoveAll(x => x.Id == customer.Id);

                //await CustomersGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Customer: {customer.Name} has been deleted.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request arg)
        {
            //NewCustomer.ProfileClaim = App.User.Profile;
            //NewCustomer.UpdatedBy = App.User.UserName;

            try
            {
                //var result = await createCustomer.Do(NewCustomer);
                //NewCustomer = new CreateCustomer.Request();

                //App.Customers.Add(result);
                //await CustomersGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Success!", $"Customer: {result.Name} has been added.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }






    }
}
