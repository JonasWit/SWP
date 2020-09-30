using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class LegalBlazorApp : BlazorAppBase
    {
        private GetClient GetClient => serviceProvider.GetService<GetClient>();
        private GetClients GetClients => serviceProvider.GetService<GetClients>();
        private UserManager<IdentityUser> UserManager => serviceProvider.GetService<UserManager<IdentityUser>>();

        private readonly NotificationService notificationService;

        private readonly IServiceProvider serviceProvider;

        public event EventHandler ActiveClientChanged;

        public CalendarPage CalendarPage { get; }
        public CasesPage CasesPage { get; }
        public ClientPage ClientsPage { get; }
        public MyAppPage MyAppPage { get; }
        public ErrorPage ErrorPage { get; }
        public NoProfileWarning NoProfileWarning { get; }
        public FinancePage FinancePage { get; }
        public ProductivityPage ProductivityPage { get; }
        public ClientJobsPage ClientJobsPage { get; }
        public ArchivePage ArchivePage { get; }

        private ClientViewModel activeClient;
        public string SelectedClientString { get; set; }

        public ClientViewModel ActiveClient
        {
            get => activeClient;
            set
            {
                activeClient = value;
                if (activeClient != null)
                {
                    ActiveClientWithData = GetClient.Get(activeClient.Id);
                }
            }
        }

        public LegalBlazorApp(
            CalendarPage calendarPanel,
            CasesPage casesPanel,
            ClientPage clientsPanel,
            MyAppPage myAppPanel,
            ErrorPage errorPage,
            NoProfileWarning noProfileWarning,
            NotificationService notificationService,
            FinancePage financePage,
            ProductivityPage productivityPage,
            ClientJobsPage clientJobsPage,
            ArchivePage archivePage,
            IServiceProvider serviceProvider)
        {
            this.notificationService = notificationService;
            this.serviceProvider = serviceProvider;

            FinancePage = financePage;
            ProductivityPage = productivityPage;
            ClientJobsPage = clientJobsPage;
            ArchivePage = archivePage;
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

                User.User = await UserManager.FindByIdAsync(ActiveUserId);
                User.Claims = await UserManager.GetClaimsAsync(User.User) as List<Claim>;
                User.Roles = await UserManager.GetRolesAsync(User.User) as List<string>;
                User.RelatedUsers = await UserManager.GetUsersForClaimAsync(User.ProfileClaim);

                Clients = GetClients.GetClientsWithoutData(User.Profile)?.Select(x => (ClientViewModel)x).ToList();

                await InitializePages();
            }
        }

        public async Task RefreshRelatedUsers() => User.RelatedUsers = await UserManager.GetUsersForClaimAsync(User.ProfileClaim);

        private async Task InitializePages()
        {
            await CalendarPage.Initialize(this);
            await CasesPage.Initialize(this);
            await ClientsPage.Initialize(this);
            await MyAppPage.Initialize(this);
            await ErrorPage.Initialize(this);
            await NoProfileWarning.Initialize(this);
            await FinancePage.Initialize(this);
            await ProductivityPage.Initialize(this);
            await ClientJobsPage.Initialize(this);
            await ArchivePage.Initialize(this);
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
            Productivity = 6,
            ClientJobs = 7,
            Archive = 8,
        }

        public ClientViewModel ActiveClientWithData { get; private set; }
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
                ClientViewModel newModel = GetClient.Get(ActiveClient.Id);

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
                Clients = GetClients.GetClientsWithoutData(User.Profile, true).Select(x => (ClientViewModel)x).ToList();

                ActiveClient = null;
                ActiveClientWithData = null;
                SelectedClientString = null;
            }
            catch (Exception ex)
            {
                ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                ForceRefresh();
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
