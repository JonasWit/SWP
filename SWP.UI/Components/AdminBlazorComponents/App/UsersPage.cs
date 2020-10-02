using Microsoft.AspNetCore.Identity;
using Radzen;
using Radzen.Blazor;
using SWP.Domain.Enums;
using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class UsersPage : BlazorPageBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AdminBlazorApp App { get; private set; }
        public int SelectedRole { get; set; } = 1;
        public bool Lock { get; set; }
        public string SelectedApplicationClaim { get; set; } = "";
        public string SelectedStatusClaim { get; set; } = "";
        public string ProfileClaimName { get; set; } = "";
        public RadzenGrid<Claim> ClaimsGrid { get; set; }
        public List<string> StatusClaims => Enum.GetNames(typeof(UserStatus)).ToList();
        public List<string> Claims => Enum.GetNames(typeof(ApplicationType)).ToList();
        public List<string> Roles => Enum.GetNames(typeof(RoleType)).ToList();
        public bool Loading { get; set; }
        public UserModel SelectedUser { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();

        public class UserModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public bool LockoutEnabled { get; set; }
            public DateTimeOffset? LockoutEnd { get; set; }
            public int UserRoleInt
            {
                get
                {
                    return (int)UserRole;
                }
                set
                {
                    UserRoleInt = value;
                    UserRole = (RoleType)UserRoleInt;
                }
            }

            public RoleType UserRole { get; set; } = RoleType.Users;
            public string ProfileClaim => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;
            public List<Claim> Claims { get; set; } = new List<Claim>();
        }

        public UsersPage(UserManager<IdentityUser> userManager, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.userManager = userManager;
        }

        public override async Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;

            await GetUsers();
            SelectedRole = SelectedUser.UserRoleInt;
        }

        public async Task GetUsers()
        {
            Users = new List<UserModel>();

            var users = userManager.Users.Select(x => new IdentityUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PasswordHash = "*****",
            }).ToList();

            foreach (var user in users)
            {
                Users.Add(await GetUser(user.Id));
            }

            if (SelectedUser == null)
            {
                SelectedUser = Users.FirstOrDefault();
            }
            else 
            {
                SelectedUser = Users.FirstOrDefault(x => x.Id == SelectedUser.Id);
            } 
        }

        private async Task<UserModel> GetUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var claims = await userManager.GetClaimsAsync(user) as List<Claim>;
            var role = await userManager.GetRolesAsync(user) as List<string>;

            return new UserModel
            {
                Name = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Claims = claims,
                UserRole = (RoleType)Enum.Parse(typeof(RoleType), role.First(), true),
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled
            };
        }

        public void RowSelected(object args)
        {
            SelectedUser = (UserModel)args;
            SelectedRole = SelectedUser.UserRoleInt;
            Lock = SelectedUser.LockoutEnd != null ? true : false;
        }

        public async Task RoleChanged(int input)
        {
            if (SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
            {
                SelectedRole = 0;
                App.ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be changed!", 5000);
                return;
            }

            try
            {
                if (SelectedUser.UserRoleInt == input)
                {
                    return;
                }

                var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                var selectedUserRoles = await userManager.GetRolesAsync(userIdentity) as List<string>;

                foreach (var role in selectedUserRoles)
                {
                    await userManager.RemoveFromRoleAsync(userIdentity, role);
                }

                await userManager.AddToRoleAsync(userIdentity, ((RoleType)input).ToString());
                await GetUsers();

                App.ShowNotification(NotificationSeverity.Success, "Done!", $"Role of: {SelectedUser.Name} has been changed to: {(RoleType)input}", 5000);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task ApplicationProfileChanged(int input)
        {
            try
            {
                var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                var selectedUserRoles = await userManager.GetRolesAsync(userIdentity) as List<string>;

                foreach (var role in selectedUserRoles)
                {
                    await userManager.RemoveFromRoleAsync(userIdentity, role);
                }

                await userManager.AddToRoleAsync(userIdentity, ((RoleType)input).ToString());
                await GetUsers();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task DeleteClaimRow(Claim claim)
        {
            if (claim.Type == "Root") return;

            if (SelectedUser.Claims.Any(x => x.Value == claim.Value))
            {
                try
                {
                    var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                    var deletedClaim = SelectedUser.Claims.FirstOrDefault(x => x.Value == claim.Value);
                    var result = await userManager.RemoveClaimAsync(userIdentity, deletedClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        SelectedUser = await GetUser(SelectedUser.Id);
                    }
                }
                catch (Exception ex)
                {
                    App.ErrorPage.DisplayMessage(ex);
                }
            }

            await ClaimsGrid.Reload();
        }

        public async Task AddApplicationClaim()
        {
            if (!SelectedUser.Claims.Any(x => x.Value == SelectedApplicationClaim) &&
                !string.IsNullOrEmpty(SelectedApplicationClaim))
            {
                try
                {
                    var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Application.ToString(), SelectedApplicationClaim);
                    var result = await userManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        SelectedUser = await GetUser(SelectedUser.Id);
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    App.ErrorPage.DisplayMessage(ex);
                }
            }
        }

        public async Task AddStatusClaim()
        {
            if (!SelectedUser.Claims.Any(x => x.Value == SelectedStatusClaim) &&
                !string.IsNullOrEmpty(SelectedStatusClaim))
            {
                try
                {
                    var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Status.ToString(), SelectedStatusClaim);
                    var result = await userManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        SelectedUser = await GetUser(SelectedUser.Id);
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    App.ErrorPage.DisplayMessage(ex);
                }
            }
        }

        public async Task AddProfileClaim()
        {
            if (!SelectedUser.Claims.Any(x => x.Value == ProfileClaimName) &&
                !string.IsNullOrEmpty(ProfileClaimName))
            {
                try
                {
                    var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Profile.ToString(), ProfileClaimName);
                    var result = await userManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        SelectedUser = await GetUser(SelectedUser.Id);
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    App.ErrorPage.DisplayMessage(ex);
                }
            }
        }

        public async Task LockUser(bool input)
        {
            try
            {
                if (SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
                {
                    Lock = false;
                    App.ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be locked!", 5000);
                    return;
                }

                var userIdentity = await userManager.FindByIdAsync(SelectedUser.Id);

                if (input)
                {
                    if (SelectedUser.LockoutEnd != null) return;

                    var result = await userManager.SetLockoutEndDateAsync(userIdentity, DateTime.Now.AddYears(+25));

                    if (result.Succeeded)
                    {
                        App.ShowNotification(NotificationSeverity.Warning, "Done!", $"User: {SelectedUser.Name} has been locked!", 5000);
                    }
                }
                else
                {
                    if (SelectedUser.LockoutEnd == null) return;

                    var result = await userManager.SetLockoutEndDateAsync(userIdentity, null);

                    if (result.Succeeded)
                    {
                        App.ShowNotification(NotificationSeverity.Info, "Done!", $"User: {SelectedUser.Name} has been unlocked!", 5000);
                    }
                }
                
                await GetUsers();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task DeleteUser(UserModel user)
        {
            if (SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
            {
                SelectedRole = 0;
                App.ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be deleted!", 5000);
                return;
            }

            try
            {



            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }
    }
}
