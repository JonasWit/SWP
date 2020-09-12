using Microsoft.AspNetCore.Identity;
using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class AdminBlazorApp : BlazorAppBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersPage UsersPage { get; }

        public AdminBlazorApp(
            UserManager<IdentityUser> userManager,
            UsersPage usersPage)
        {
            this.userManager = userManager;

            UsersPage = usersPage;
        }

        public Panels ActivePanel { get; private set; } = Panels.Users;

        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        public enum Panels
        {
            Users = 0,
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
        }






    }
}
