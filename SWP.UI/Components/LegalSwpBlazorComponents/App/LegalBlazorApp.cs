using Microsoft.AspNetCore.Identity;
using Radzen;
using SWP.Application.LegalSwp.Customers;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class LegalBlazorApp : BlazorAppBase
    {
        private readonly GetCustomer getCustomer;
        private readonly GetCustomers getCustomers;
        private readonly UserManager<IdentityUser> userManager;
        private readonly NotificationService notificationService;

        public event EventHandler ActiveCustomerChanged;

        public CalendarPage CalendarPage { get; }
        public CasesPage CasesPage { get; }
        public CustomersPage CustomersPage { get; }
        public MyAppPage MyAppPage { get; }
        public ErrorPage ErrorPage { get; }
        public NoProfileWarning NoProfileWarning { get; }

        private CustomerViewModel activeCustomer;

        public CustomerViewModel ActiveCustomer
        {
            get => activeCustomer; 
            set
            {
                activeCustomer = value;
                if (activeCustomer != null)
                {
                    ActiveCustomerWithData = getCustomer.Get(activeCustomer.Id, User.Profile);
                }
            }
        }

        public LegalBlazorApp(
            GetCustomer getCustomer,
            GetCustomers getCustomers,
            UserManager<IdentityUser> userManager,
            CalendarPage calendarPanel,
            CasesPage casesPanel,
            CustomersPage customersPanel,
            MyAppPage myAppPanel,
            ErrorPage errorPage,
            NoProfileWarning noProfileWarning,
            NotificationService notificationService)
        {
            this.getCustomer = getCustomer;
            this.getCustomers = getCustomers;
            this.userManager = userManager;
            this.notificationService = notificationService;

            CalendarPage = calendarPanel;
            CasesPage = casesPanel;
            CustomersPage = customersPanel;
            MyAppPage = myAppPanel;
            ErrorPage = errorPage;
            NoProfileWarning = noProfileWarning;
        }

        public override async Task Initialize(string activeUserId)
        {
            if (Initialized)
            {
                return;
            }
            else
            {
                ActiveUserId = activeUserId;

                User.User = await userManager.FindByIdAsync(ActiveUserId);
                User.Claims = await userManager.GetClaimsAsync(User.User) as List<Claim>;
                User.Roles = await userManager.GetRolesAsync(User.User) as List<string>;

                Customers = getCustomers.GetCustomersWithoutData(User.Profile)?.Select(x => (CustomerViewModel)x).ToList();
                //ActiveCustomer = Customers.FirstOrDefault();

                //For the first run
                //if (ActiveCustomer != null)
                //{
                //    ActiveCustomerWithData = getCustomer.Get(ActiveCustomer.Id, User.Profile);
                //}

                //RefreshCustomers();
                InitializePanels();
            }
        }

        private void InitializePanels()
        {
            CalendarPage.Initialize(this);
            CasesPage.Initialize(this);
            CustomersPage.Initialize(this);
            MyAppPage.Initialize(this);
            ErrorPage.Initialize(this);
            NoProfileWarning.Initialize(this);
        }

        #region Main Component

        public enum Panels
        {
            Customers = 0,
            Calendar = 1,
            Cases = 2,
            MyApp = 3,
            ErrorPage = 4
        }

        public CustomerViewModel ActiveCustomerWithData { get; set; }
        public Panels ActivePanel { get; private set; } = Panels.MyApp;
        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();

        public void RedirectToCase(int id)
        {
            if (!ActiveCustomerWithData.Cases.Any(x => x.Id == id))
            {
                return;
            }

            SetActivePanel(LegalBlazorApp.Panels.Cases);
            CasesPage.SetActivePanel(CasesPage.Panels.Admin);
            ActiveCustomerWithData.SelectedCase = ActiveCustomerWithData.Cases.FirstOrDefault(x => x.Id == id);
            OnCallStateHasChanged(null);
        }

        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        public void ForceRefresh() => OnCallStateHasChanged(null);

        public void ShowNotification(NotificationSeverity severity, string summary, string detail, int duration) => 
            notificationService.Notify(new NotificationMessage() { Severity = severity, Summary = summary, Detail = detail, Duration = duration });
        
        public void ThrowTestException()
        {
            try
            {
                throw new Exception("Test Exception - Ups!");
            }
            catch (Exception ex)
            {
                ErrorPage.DisplayMessage(ex);
            }
        }

        public void RefreshCustomerWithData()
        {
            if (ActiveCustomer != null)
            {
                CustomerViewModel newModel = getCustomer.Get(ActiveCustomer.Id, User.Profile);

                if (ActiveCustomerWithData.SelectedCase != null)
                {
                    newModel.SelectedCase = newModel.Cases.FirstOrDefault(x => x.Id == ActiveCustomerWithData.SelectedCase.Id);
                }

                ActiveCustomerWithData = newModel;
            }
        }

        public void RefreshCustomers()
        {
            try
            {
                Customers = getCustomers.GetCustomersWithoutData(User.Profile).Select(x => (CustomerViewModel)x).ToList();

                if (Customers.ToList().Count == 1)
                {
                    ActiveCustomer = Customers.FirstOrDefault();
                }

                if (ActiveCustomer == null || !Customers.Any(x => x.Id == ActiveCustomer.Id))
                {
                    if (Customers.Count() == 0)
                    {
                        ActiveCustomer = null;
                        ActiveCustomerWithData = null;
                    }
                    else
                    {
                        ActiveCustomer = Customers.FirstOrDefault();
                    }
                }
                else
                {
                    ActiveCustomer = Customers.FirstOrDefault(x => x.Id == ActiveCustomer.Id);
                }
            }
            catch (Exception ex)
            {
                ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                OnCallStateHasChanged(null);
            }
        }

        public void ActiveCustomerChange(object value)
        {
            if (value != null)
            {
                ActiveCustomer = Customers.FirstOrDefault(x => x.Id == int.Parse(value.ToString()));
            }
            else
            {
                if (ActivePanel != Panels.Calendar)
                {
                    SetActivePanel(Panels.MyApp);
                }

                ActiveCustomer = null;
                ActiveCustomerWithData = null;
            }

            ActiveCustomerChanged?.Invoke(this, null);
        }

        #endregion

    }
}
