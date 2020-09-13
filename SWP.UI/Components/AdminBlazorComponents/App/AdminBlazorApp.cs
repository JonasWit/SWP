using Microsoft.AspNetCore.Identity;
using SWP.UI.BlazorApp;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class AdminBlazorApp : BlazorAppBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersPage UsersPage { get; }
        public DatabasePage DatabasePage { get; }
        public ErrorPage ErrorPage { get; }
        public ApplicationsPage ApplicationsPage { get; }

        public AdminBlazorApp(
            UserManager<IdentityUser> userManager,
            UsersPage usersPage,
            DatabasePage databasePage,
            ErrorPage errorPage,
            ApplicationsPage applicationsPage)
        {
            this.userManager = userManager;

            UsersPage = usersPage;
            DatabasePage = databasePage;
            ErrorPage = errorPage;
            ApplicationsPage = applicationsPage;
        }

        public Panels ActivePanel { get; private set; } = Panels.Users;

        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        public enum Panels
        {
            Users = 0,
            Error = 1,
            Database = 2,
            Applications = 3
        }

        public void ForceRefresh() => OnCallStateHasChanged(null);

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

                await InitializePanels();
            }
        }

        private async Task InitializePanels()
        {
            await UsersPage.Initialize(this);
            await DatabasePage.Initialize(this);
            await ErrorPage.Initialize(this);
            await ApplicationsPage.Initialize(this);
        }






    }
}
