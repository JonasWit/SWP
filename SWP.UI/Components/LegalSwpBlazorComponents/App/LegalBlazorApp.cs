using Microsoft.AspNetCore.Identity;
using Radzen;
using SWP.Application.LegalSwp.Clients;
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
        private readonly GetClient getClient;
        private readonly GetClients getClients;
        private readonly UserManager<IdentityUser> userManager;
        private readonly NotificationService notificationService;
      

        public event EventHandler ActiveClientChanged;

        public int NotificationDuration { get; }
        public CalendarPage CalendarPage { get; }
        public CasesPage CasesPage { get; }
        public ClientPage ClientsPage { get; }
        public MyAppPage MyAppPage { get; }
        public ErrorPage ErrorPage { get; }
        public NoProfileWarning NoProfileWarning { get; }
        public FinancePage FinancePage { get; }

        private ClientViewModel activeClient;

        public ClientViewModel ActiveClient
        {
            get => activeClient; 
            set
            {
                activeClient = value;
                if (activeClient != null)
                {
                    ActiveClientWithData = getClient.Get(activeClient.Id, User.Profile);
                }
            }
        }

        public LegalBlazorApp(
            GetClient getClient,
            GetClients getClients,
            UserManager<IdentityUser> userManager,
            CalendarPage calendarPanel,
            CasesPage casesPanel,
            ClientPage clientsPanel,
            MyAppPage myAppPanel,
            ErrorPage errorPage,
            NoProfileWarning noProfileWarning,
            NotificationService notificationService,
            FinancePage financePage)
        {
            this.getClient = getClient;
            this.getClients = getClients;
            this.userManager = userManager;
            this.notificationService = notificationService;

            NotificationDuration = 3000;

            FinancePage = financePage;
            CalendarPage = calendarPanel;
            CasesPage = casesPanel;
            ClientsPage = clientsPanel;
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

                Clients = getClients.GetClientsWithoutData(User.Profile)?.Select(x => (ClientViewModel)x).ToList();

                InitializePanels();
            }
        }

        private void InitializePanels()
        {
            CalendarPage.Initialize(this);
            CasesPage.Initialize(this);
            ClientsPage.Initialize(this);
            MyAppPage.Initialize(this);
            ErrorPage.Initialize(this);
            NoProfileWarning.Initialize(this);
            FinancePage.Initialize(this);
        }

        #region Main Component

        public enum Panels
        {
            Clients = 0,
            Calendar = 1,
            Cases = 2,
            MyApp = 3,
            ErrorPage = 4,
            Finance = 5,
        }

        public ClientViewModel ActiveClientWithData { get; set; }
        public Panels ActivePanel { get; private set; } = Panels.MyApp;
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();

        public void RedirectToCase(int id)
        {
            if (!ActiveClientWithData.Cases.Any(x => x.Id == id))
            {
                return;
            }

            SetActivePanel(LegalBlazorApp.Panels.Cases);
            ActiveClientWithData.SelectedCase = ActiveClientWithData.Cases.FirstOrDefault(x => x.Id == id);
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

        public void RefreshClientWithData()
        {
            if (ActiveClient != null)
            {
                ClientViewModel newModel = getClient.Get(ActiveClient.Id, User.Profile);

                if (ActiveClientWithData.SelectedCase != null)
                {
                    newModel.SelectedCase = newModel.Cases.FirstOrDefault(x => x.Id == ActiveClientWithData.SelectedCase.Id);
                }

                ActiveClientWithData = newModel;
            }
        }

        public void RefreshClients()
        {
            try
            {
                Clients = getClients.GetClientsWithoutData(User.Profile).Select(x => (ClientViewModel)x).ToList();

                if (Clients.ToList().Count == 1)
                {
                    ActiveClient = Clients.FirstOrDefault();
                }

                if (ActiveClient == null || !Clients.Any(x => x.Id == ActiveClient.Id))
                {
                    if (Clients.Count() == 0)
                    {
                        ActiveClient = null;
                        ActiveClientWithData = null;
                    }
                    else
                    {
                        ActiveClient = Clients.FirstOrDefault();
                    }
                }
                else
                {
                    ActiveClient = Clients.FirstOrDefault(x => x.Id == ActiveClient.Id);
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

        public void ActiveClientChange(object value)
        {
            if (value != null)
            {
                ActiveClient = Clients.FirstOrDefault(x => x.Id == int.Parse(value.ToString()));
            }
            else
            {
                if (ActivePanel != Panels.Calendar)
                {
                    SetActivePanel(Panels.MyApp);
                }

                ActiveClient = null;
                ActiveClientWithData = null;
            }

            ActiveClientChanged?.Invoke(this, null);
        }

        #endregion

    }
}
