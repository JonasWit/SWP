using Microsoft.AspNetCore.Identity;
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
        public AdminBlazorApp App { get; private set; }

        public int SelectedRole { get; set; } = 1;
        public string SelectedApplicationClaim { get; set; } = "";
        public string ProfileClaimName { get; set; } = "";

        public RadzenGrid<Claim> ClaimsGrid { get; set; }

        private readonly UserManager<IdentityUser> userManager;

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
            public List<Claim> Claims { get; set; } = new List<Claim>();
        }

        public UsersPage(UserManager<IdentityUser> userManager)
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
                PasswordHash = "*****"
            }).ToList();

            foreach (var user in users)
            {
                Users.Add(await GetUser(user.Id));
            }

            if (SelectedUser == null)
            {
                SelectedUser = Users.FirstOrDefault();
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
            };
        }

        public void RowSelected(object args)
        {
            SelectedUser = (UserModel)args;
            SelectedRole = SelectedUser.UserRoleInt;
        }

        public async Task RoleChanged(int input)
        {
            if (Loading) return;
            else Loading = true;

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
                await GetUser(SelectedUser.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Loading = false;
            }
        }

        public async Task ApplicationProfileChanged(int input)
        {
            if (Loading) return;
            else Loading = true;

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

                throw;
            }
            finally
            {
                Loading = false;
            }
        }

        public async Task DeleteClaimRow(Claim claim)
        {
            if (Loading) return;
            else Loading = true;

            if (claim.Type == "Root")
            {
                Loading = false;
                return;
            }

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
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Loading = false;
                }
            }
            else
            {

            }

            await ClaimsGrid.Reload();
        }

        public async Task AddApplicationClaim()
        {
            if (Loading) return;
            else Loading = true;

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
                    throw;
                }
                finally
                {
                    Loading = false;
                }
            }
            else
            {
                Loading = false;
            }
        }

        public async Task AddProfileClaim()
        {
            if (Loading) return;
            else Loading = true;

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
                    throw;
                }
                finally
                {
                    Loading = false;
                }
            }
            else
            {
                Loading = false;
            }
        }

        public async Task LockUser()
        { 
                //todo: wiadomo
        
        
        }

        public async Task UnlockUser()
        {
            //todo: wiadomo


        }


    }
}
