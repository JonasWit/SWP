﻿using Radzen.Blazor;
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

        public LegalBlazorApp App { get; private set; }

        public CustomersPage(
            DeleteCustomer deleteCustomer,
            UpdateCustomer updateCustomer,
            CreateCustomer createCustomer,
            CreateCustomerJob createCustomerJob,
            DeleteCustomerJob deleteCustomerJob,
            UpdateCustomerJob updateCustomerJob)
        {
            this.deleteCustomer = deleteCustomer;
            this.updateCustomer = updateCustomer;
            this.createCustomer = createCustomer;
            this.createCustomerJob = createCustomerJob;
            this.deleteCustomerJob = deleteCustomerJob;
            this.updateCustomerJob = updateCustomerJob;
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
            if (App.Loading) return;
            else App.Loading = true;

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

                if (App.ActiveCustomer.Id == result.Id)
                {
                    App.ActiveCustomer = result;
                }
                else
                {
                    App.Customers[App.Customers.FindIndex(x => x.Id == result.Id)] = result;
                }
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

        public void SaveCustomerRow(CustomerViewModel customer) => CustomersGrid.UpdateRow(customer);

        public void CancelCustomerEdit(CustomerViewModel customer)
        {
            CustomersGrid.CancelEditRow(customer);
            App.RefreshCustomers();
        }

        public async Task DeleteCustomerRow(CustomerViewModel customer)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                await deleteCustomer.Delete(customer.Id);
                //todo check if needed maybe just remove from list
                App.RefreshCustomers();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                App.Loading = false;
            }

            await CustomersGrid.Reload();
        }

        public async Task SubmitNewCustomer(CreateCustomer.Request arg)
        {
            if (App.Loading) return;
            else App.Loading = true;

            NewCustomer.ProfileClaim = App.User.Profile;
            NewCustomer.UpdatedBy = App.User.UserName;

            try
            {
                await createCustomer.Do(NewCustomer);

                NewCustomer = new CreateCustomer.Request();
                App.RefreshCustomers();
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

        #region Jobs

        public async Task SubmitNewCustomerJob(CreateCustomerJob.Request arg)
        {
            if (App.Loading) return;
            else App.Loading = true;

            try
            {
                NewCustomerJob.ProfileClaim = App.User.Profile;
                NewCustomerJob.CustomerId = App.ActiveCustomer.Id;
                NewCustomerJob.UpdatedBy = App.User.UserName;

                await createCustomerJob.Create(NewCustomerJob);
                NewCustomerJob = new CreateCustomerJob.Request();
                App.RefreshCustomers();
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
                App.RefreshCustomers();
                App.RefreshCustomers();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                App.Loading = false;
            }

            await CustomersGrid.Reload();
        }

        #endregion








    }
}
