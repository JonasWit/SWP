using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Customers;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CustomersPage : BlazorPageBase
    {
        public enum Panels
        {
            Admin = 0,
            Jobs = 1,
            Main = 2
        }

        private readonly DeleteCustomer deleteCustomer;
        private readonly UpdateCustomer updateCustomer;
        private readonly CreateCustomer createCustomer;
        private readonly CreateCustomerJob createCustomerJob;
        private readonly DeleteCustomerJob deleteCustomerJob;
        private readonly UpdateCustomerJob updateCustomerJob;
        private readonly GetCustomers getCustomers;

        public LegalBlazorApp App { get; private set; }

        public CustomersPage(
            DeleteCustomer deleteCustomer,
            UpdateCustomer updateCustomer,
            CreateCustomer createCustomer,
            CreateCustomerJob createCustomerJob,
            DeleteCustomerJob deleteCustomerJob,
            UpdateCustomerJob updateCustomerJob,
            GetCustomers getCustomers)
        {
            this.deleteCustomer = deleteCustomer;
            this.updateCustomer = updateCustomer;
            this.createCustomer = createCustomer;
            this.createCustomerJob = createCustomerJob;
            this.deleteCustomerJob = deleteCustomerJob;
            this.updateCustomerJob = updateCustomerJob;
            this.getCustomers = getCustomers;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public Panels ActivePanel { get; set; }
        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        public CreateCustomer.Request NewCustomer { get; set; } = new CreateCustomer.Request();
        public CreateCustomerJob.Request NewCustomerJob { get; set; } = new CreateCustomerJob.Request();
        public RadzenGrid<CustomerViewModel> CustomersGrid { get; set; }
        public RadzenGrid<CustomerJobViewModel> CustomersJobsGrid { get; set; }

        #region Customer

        public void EditCustomerRow(CustomerViewModel customer) => CustomersGrid.EditRow(customer);

        public async Task OnUpdateCustomerRow(CustomerViewModel customer)
        {
            try
            {
                var result = await updateCustomer.Update(new UpdateCustomer.Request
                {
                    Id = customer.Id,
                    Active = customer.Active,
                    Address = customer.Address,
                    Email = customer.Email,
                    Name = customer.Name,
                    PhoneNumber = customer.PhoneNumber,
                    ProfileClaim = App.User.Profile,
                    UpdatedBy = App.User.UserName
                });

                if (App.ActiveCustomer != null && App.ActiveCustomer.Id == result.Id)
                {
                    App.ActiveCustomer = result;
                }
                else
                {
                    App.Customers[App.Customers.FindIndex(x => x.Id == result.Id)] = result;
                }

                await CustomersGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Customer: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveCustomerRow(CustomerViewModel customer) => CustomersGrid.UpdateRow(customer);

        public void CancelCustomerEdit(CustomerViewModel customer)
        {
            CustomersGrid.CancelEditRow(customer);
            App.RefreshCustomers();
        }

        public async Task DeleteCustomerRow(CustomerViewModel customer)
        {
            try
            {
                await deleteCustomer.Delete(customer.Id);
                App.Customers.RemoveAll(x => x.Id == customer.Id);

                await CustomersGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Customer: {customer.Name} has been deleted.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewCustomer(CreateCustomer.Request arg)
        {
            NewCustomer.ProfileClaim = App.User.Profile;
            NewCustomer.UpdatedBy = App.User.UserName;

            try
            {
                var result = await createCustomer.Do(NewCustomer);
                NewCustomer = new CreateCustomer.Request();

                App.Customers.Add(result);
                await CustomersGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Customer: {result.Name} has been added.", 2000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion

        #region Jobs

        public async Task SubmitNewCustomerJob(CreateCustomerJob.Request arg)
        {
            try
            {
                NewCustomerJob.ProfileClaim = App.User.Profile;
                NewCustomerJob.CustomerId = App.ActiveCustomer.Id;
                NewCustomerJob.UpdatedBy = App.User.UserName;

                var result = await createCustomerJob.Create(NewCustomerJob);
                NewCustomerJob = new CreateCustomerJob.Request();

                if (App.ActiveCustomerWithData != null)
                {
                    App.ActiveCustomerWithData.Jobs.Add(result);
                }

                await CustomersJobsGrid.Reload();
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

        public void EditCustomerJobRow(CustomerJobViewModel customerJob) => CustomersJobsGrid.EditRow(customerJob);

        public async Task OnUpdateCustomerJobRow(CustomerJobViewModel customerJob)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                var result = await updateCustomerJob.Update(new UpdateCustomerJob.Request
                {
                    Id = customerJob.Id,
                    Active = customerJob.Active,
                    Description = customerJob.Description,
                    Name = customerJob.Name,
                    Priority = customerJob.Priority,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveCustomerWithData.Jobs[App.ActiveCustomerWithData.Jobs.FindIndex(x => x.Id == result.Id)] = result;
                await CustomersJobsGrid.Reload();
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

        public void SaveCustomerJobRow(CustomerJobViewModel customerJob) => CustomersJobsGrid.UpdateRow(customerJob);

        public void CancelCustomerJobEdit(CustomerJobViewModel customerJob)
        {
            CustomersJobsGrid.CancelEditRow(customerJob);
            App.RefreshCustomers();
        }

        public async Task DeleteCustomerJobRow(CustomerJobViewModel customerJob)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                await deleteCustomerJob.Delete(customerJob.Id);

                if (App.ActiveCustomerWithData != null)
                {
                    App.ActiveCustomerWithData.Jobs.RemoveAll(x => x.Id == customerJob.Id);
                }

                await CustomersJobsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Task: {customerJob.Name} has been deleted.", 2000);
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
