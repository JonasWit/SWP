using Microsoft.AspNetCore.Identity;
using SWP.Application.LegalSwp.Customers;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class LegalSwpApp
    {
        private readonly GetCustomer getCustomer;
        private readonly GetCustomers getCustomers;
        private readonly UserManager<IdentityUser> userManager;

        public event EventHandler RefreshUI;

        public CalendarPanel CalendarPanel { get; }
        public CasesPanel CasesPanel { get; }
        public CustomersPanel CustomersPanel { get; }
        public MyAppPanel MyAppPanel { get; }
        public string ActiveUserId { get; private set; }
        public bool Loading { get; set; }

        public LegalSwpApp(
            GetCustomer getCustomer,
            GetCustomers getCustomers,
            UserManager<IdentityUser> userManager,
            CalendarPanel calendarPanel,
            CasesPanel casesPanel,
            CustomersPanel customersPanel,
            MyAppPanel myAppPanel)      
        {
            this.getCustomer = getCustomer;
            this.getCustomers = getCustomers;
            this.userManager = userManager;

            CalendarPanel = calendarPanel;
            CasesPanel = casesPanel;
            CustomersPanel = customersPanel;
            MyAppPanel = myAppPanel;
        }

        public async Task Initialize(string activeUserId)
        {
            ActiveUserId = activeUserId;

            User.User = await userManager.FindByIdAsync(ActiveUserId);
            User.Claims = await userManager.GetClaimsAsync(User.User) as List<Claim>;
            User.Roles = await userManager.GetRolesAsync(User.User) as List<string>;

            Customers = getCustomers.GetCustomersWithoutData(User.Profile)?.Select(x => (CustomerViewModel)x).ToList();
            ActiveCustomer = Customers.FirstOrDefault();

            //For the first run
            if (ActiveCustomer != null)
            {
                ActiveCustomerWithData = getCustomer.Get(ActiveCustomer.Id, User.Profile);
            }

            RefreshCustomers();
            InitializePanels();
        }

        private void InitializePanels()
        {
            CalendarPanel.Initialize(this);
            CasesPanel.Initialize(this);
            CustomersPanel.Initialize(this);
            MyAppPanel.Initialize(this);
        }

        #region Main Component

        public enum Panels
        {
            Customers = 0,
            Calendar = 1,
            Cases = 2,
            MyApp = 3,
        }

        public CustomerViewModel ActiveCustomerWithData { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public Panels ActivePanel { get; set; } = Panels.MyApp;
        public IEnumerable<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();
        public void RedirectToCase(int id)
        {
            if (!ActiveCustomerWithData.Cases.Any(x => x.Id == id))
            {
                return;
            }

            SetActivePanel(LegalSwpApp.Panels.Cases);
            CasesPanel.SetActivePanel(CasesPanel.Panels.Admin);
            ActiveCustomerWithData.SelectedCase = ActiveCustomerWithData.Cases.FirstOrDefault(x => x.Id == id);
            RefreshUI?.Invoke(this, null);
        }
        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        #region NavBar

        public bool collapseNavMenu = true;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public void ToggleNavMenu() => collapseNavMenu = !collapseNavMenu;

        #endregion

        private CustomerViewModel activeCustomer;

        public CustomerViewModel ActiveCustomer
        {
            get { return activeCustomer; }
            set
            {
                activeCustomer = value;
                if (activeCustomer != null)
                {
                    ActiveCustomerWithData = getCustomer.Get(activeCustomer.Id, User.Profile);
                }
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                RefreshUI?.Invoke(this, null);
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
        }

        #endregion

    }
}
