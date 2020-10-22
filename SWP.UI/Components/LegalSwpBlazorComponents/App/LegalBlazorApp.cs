using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.Dialogs;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class LegalBlazorApp : BlazorAppBase, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private ClientViewModel _activeClient;
        private readonly NotificationService _notificationService;

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
        public ClientDetailsPage ClientDetailsPage { get; }
        public string SelectedClientString { get; set; }
        public ClientViewModel ActiveClientWithData { get; private set; }
        public Panels ActivePanel { get; private set; } = Panels.MyApp;
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();

        public ClientViewModel ActiveClient
        {
            get => _activeClient;
            set
            {
                _activeClient = value;
                if (_activeClient != null) FireUpdatesAfterActiveClientChange();
            }
        }

        public LegalBlazorApp(
            CalendarPage calendarPanel,
            CasesPage casesPanel,
            ClientPage clientsPanel,
            MyAppPage myAppPanel,
            ErrorPage errorPage,
            NoProfileWarning noProfileWarning,
            FinancePage financePage,
            ProductivityPage productivityPage,
            ClientJobsPage clientJobsPage,
            ArchivePage archivePage,
            ClientDetailsPage clientDetailsPage,
            IServiceProvider serviceProvider,
            NotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _notificationService = notificationService;
            FinancePage = financePage;
            ProductivityPage = productivityPage;
            ClientJobsPage = clientJobsPage;
            ArchivePage = archivePage;
            ClientDetailsPage = clientDetailsPage;
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
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                ActiveUserId = activeUserId;

                User.User = await userManager.FindByIdAsync(ActiveUserId);
                User.Claims = await userManager.GetClaimsAsync(User.User) as List<Claim>;
                User.Roles = await userManager.GetRolesAsync(User.User) as List<string>;
                User.RelatedUsers = await userManager.GetUsersForClaimAsync(User.ProfileClaim);

                Clients = getClients.GetClientsWithoutData(User.Profile)?.Select(x => (ClientViewModel)x).ToList();

                await InitializePages();
            }
        }

        private void FireUpdatesAfterActiveClientChange()
        {
            if (!Loading)
            {
                Loading = !Loading;
            }
            else
            {
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                ActiveClientWithData = getClient.Get(_activeClient.Id);
                FinancePage.GetDataForMonthFilter();
                ProductivityPage.GetDataForMonthFilter();
            }
            catch (Exception ex)
            {
                ErrorPage.DisplayMessage(ex);
            }
            finally
            {
                Loading = !Loading;
            }
        }

        public async Task RefreshRelatedUsers()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                User.RelatedUsers = await userManager.GetUsersForClaimAsync(User.ProfileClaim);
            }
            catch (Exception ex)
            {
                await ErrorPage.DisplayMessageAsync(ex);
            }
        }

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
            await ClientDetailsPage.Initialize(this);
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
            ClientDetails = 9,
        }

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

        public void ShowNotification(NotificationSeverity severity, string summary, string detail, int duration) =>
            _notificationService.Notify(new NotificationMessage() { Severity = severity, Summary = summary, Detail = detail, Duration = duration });

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
            try
            {
                if (ActiveClient != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                    ClientViewModel newModel = getClient.Get(ActiveClient.Id);

                    if (ActiveClientWithData.SelectedCase != null)
                    {
                        newModel.SelectedCase = newModel.Cases.FirstOrDefault(x => x.Id == ActiveClientWithData.SelectedCase.Id);
                    }

                    ActiveClientWithData = newModel;
                }
            }
            catch (Exception ex)
            {
                ErrorPage.DisplayMessage(ex);
            }
        }

        public void RefreshClients()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

                Clients = getClients.GetClientsWithoutData(User.Profile, true).Select(x => (ClientViewModel)x).ToList();

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

        public void Dispose()
        {

        }

        #endregion

    }
}
