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

        public AdminBlazorApp(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
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

                InitializePanels();
            }
        }

        private void InitializePanels()
        {

        }






    }
}
