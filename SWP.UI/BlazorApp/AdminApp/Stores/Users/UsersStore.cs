using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Domain.Enums;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.StatusLog;
using SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users
{
    public class UserState
    {
        public int SelectedRole { get; set; } = 1;
        public bool Lock { get; set; }
        public string SelectedApplicationClaim { get; set; } = "";
        public string SelectedStatusClaim { get; set; } = "";
        public string ProfileClaimName { get; set; } = "";
        public RadzenGrid<Claim> ClaimsGrid { get; set; }
        public RadzenGrid<UserModel> UsersGrid { get; set; }
        public List<string> StatusClaims => Enum.GetNames(typeof(UserStatus)).ToList();
        public List<string> Claims => Enum.GetNames(typeof(ApplicationType)).ToList();
        public List<string> Roles => Enum.GetNames(typeof(RoleType)).ToList();
        public bool Loading { get; set; }
        public UserModel SelectedUser { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public List<ProfileModel> AllProfiles { get; set; } = new List<ProfileModel>();
    }

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
                UserRole = (RoleType)UserRoleInt;
            }
        }
        public RoleType UserRole { get; set; } = RoleType.Users;
        public string ProfileClaim => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public bool RootClient => Claims.Any(x => x.Value == UserStatus.RootClient.ToString());
    }

    public class ProfileModel
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
    }

    [UIScopedService]
    public class UsersStore : StoreBase<UserState>
    {
        UserManager<IdentityUser> UserManager => _serviceProvider.GetService<UserManager<IdentityUser>>();

        ApplicationStore AppStore => _serviceProvider.GetRequiredService<ApplicationStore>();

        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();

        public UsersStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
            : base(serviceProvider, actionDispatcher, notificationService) { }

        public async Task Initialize()
        {
            _state.AllProfiles = await GetActiveProfiles();
            await GetUsers();
            _state.SelectedRole = _state.SelectedUser.UserRoleInt;
            BroadcastStateChange();
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case RowSelectedAction.RowSelected:
                    var rowSelectedAction = (RowSelectedAction)action;
                    RowSelected(rowSelectedAction.Arg);
                    break;
                case RoleChangedAction.RoleChanged:
                    var roleChangedAction = (RoleChangedAction)action;
                    await RoleChanged(roleChangedAction.Arg);
                    break;
                case DeleteClaimRowAction.DeleteClaimRow:
                    var deleteClaimRowAction = (DeleteClaimRowAction)action;
                    await DeleteClaimRow(deleteClaimRowAction.Arg);
                    break;
                case AddApplicationClaimAction.AddApplicationClaim:
                    await AddApplicationClaim();
                    break;
                case AddStatusClaimAction.AddStatusClaim:
                    await AddStatusClaim();
                    break;
                case AddProfileClaimAction.AddProfileClaim:
                    await AddProfileClaim();
                    break;
                case LockUserAction.LockUser:
                    var lockUserAction = (LockUserAction)action;
                    await LockUser(lockUserAction.Arg);
                    break;
                case DeleteUserAction.DeleteUser:
                    var deleteUserAction = (DeleteUserAction)action;
                    await DeleteUser(deleteUserAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private async Task<List<ProfileModel>> GetActiveProfiles()
        {
            var results = new List<ProfileModel>();
            var users = UserManager.Users.ToList();

            var id = 0;

            foreach (var user in users)
            {
                var claims = await UserManager.GetClaimsAsync(user);

                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimType.Profile.ToString())
                    {
                        if (!results.Any(x => x.ProfileName == claim.Value))
                        {
                            results.Add(new ProfileModel { Id = id, ProfileName = claim.Value });
                            id++;
                        }
                    }
                }
            }

            return results;
        }

        private async Task GetUsers()
        {
            _state.Users.Clear();

            var users = UserManager.Users.Select(x => new IdentityUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PasswordHash = "*****",
            }).ToList();

            foreach (var user in users)
            {
                _state.Users.Add(await GetUser(user.Id));
            }

            if (_state.SelectedUser == null)
            {
                _state.SelectedUser = _state.Users.FirstOrDefault();
            }
            else
            {
                _state.SelectedUser = _state.Users.FirstOrDefault(x => x.Id == _state.SelectedUser.Id);
            }
        }

        private async Task<UserModel> GetUser(string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);
            var claims = await UserManager.GetClaimsAsync(user) as List<Claim>;
            var role = await UserManager.GetRolesAsync(user) as List<string>;

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

        private void ShowErrorPage(Exception ex) => AppStore.ShowErrorPage(ex);

        #region Actions

        private void RowSelected(object args)
        {
            _state.SelectedUser = (UserModel)args;
            _state.SelectedRole = _state.SelectedUser.UserRoleInt;
            _state.Lock = _state.SelectedUser.LockoutEnd != null ? true : false;
            StatusBarStore.UpdateLogWindow($"User {_state.SelectedUser.Name} selected.");
        }

        private async Task RoleChanged(int input)
        {
            if (_state.SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
            {
                _state.SelectedRole = 0;
                ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be changed!", 5000);
                return;
            }

            try
            {
                if (_state.SelectedUser.UserRoleInt == input)
                {
                    return;
                }

                var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                var selectedUserRoles = await UserManager.GetRolesAsync(userIdentity) as List<string>;

                foreach (var role in selectedUserRoles)
                {
                    await UserManager.RemoveFromRoleAsync(userIdentity, role);
                }

                await UserManager.AddToRoleAsync(userIdentity, ((RoleType)input).ToString());
                await GetUsers();
                await _state.UsersGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Done!", $"Role of: {_state.SelectedUser.Name} has been changed to: {(RoleType)input}", 5000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        private async Task DeleteClaimRow(Claim claim)
        {
            if (claim.Type == "Root") return;

            if (_state.SelectedUser.Claims.Any(x => x.Value == claim.Value))
            {
                try
                {
                    var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                    var deletedClaim = _state.SelectedUser.Claims.FirstOrDefault(x => x.Value == claim.Value);
                    var result = await UserManager.RemoveClaimAsync(userIdentity, deletedClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        _state.SelectedUser = _state.Users.FirstOrDefault(x => x.Id == _state.SelectedUser.Id);
                    }

                    await _state.UsersGrid.Reload();
                    ShowNotification(NotificationSeverity.Success, "Done!", $"Claim: {claim.Value} has been deleted!", 5000);
                    BroadcastStateChange();
                }
                catch (Exception ex)
                {
                    StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                    ShowErrorPage(ex);
                }
            }

            await _state.ClaimsGrid.Reload();
        }

        private async Task AddApplicationClaim()
        {
            if (_state.SelectedUser.Claims.Any(x => x.Value == _state.SelectedApplicationClaim))
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"User already have this Claim!", 5000);
                return;
            }

            if (!string.IsNullOrEmpty(_state.SelectedApplicationClaim))
            {
                try
                {
                    var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Application.ToString(), _state.SelectedApplicationClaim);
                    var result = await UserManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        _state.SelectedUser = await GetUser(_state.SelectedUser.Id);

                        await _state.UsersGrid.Reload();
                        ShowNotification(NotificationSeverity.Success, "Done!", $"Application Claim: {_state.SelectedApplicationClaim} has been added!", 5000);
                        BroadcastStateChange();
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                    ShowErrorPage(ex);
                }
            }
            else
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"No claim was chosen!", 5000);
            }
        }

        private async Task AddStatusClaim()
        {
            if (_state.SelectedUser.Claims.Any(x => x.Value == _state.SelectedStatusClaim))
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"User already have this Claim!", 5000);
                return;
            }

            if (!string.IsNullOrEmpty(_state.SelectedStatusClaim))
            {
                try
                {
                    var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Status.ToString(), _state.SelectedStatusClaim);
                    var result = await UserManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        _state.SelectedUser = await GetUser(_state.SelectedUser.Id);
                        await _state.UsersGrid.Reload();
                        ShowNotification(NotificationSeverity.Success, "Done!", $"Status Claim: {_state.SelectedStatusClaim} has been added!", 5000);
                        BroadcastStateChange();
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                    ShowErrorPage(ex);
                }
            }
            else
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"No claim was chosen!", 5000);
            }
        }

        private async Task AddProfileClaim()
        {
            if (_state.SelectedUser.Claims.Any(x => x.Type == ClaimType.Profile.ToString()))
            {
                ShowNotification(NotificationSeverity.Error, "Remember!", $"User can have only one profile!", 5000);
                return;
            }

            if (!string.IsNullOrEmpty(_state.ProfileClaimName.Trim()))
            {
                try
                {
                    var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Profile.ToString(), _state.ProfileClaimName.Trim());
                    var result = await UserManager.AddClaimAsync(userIdentity, newClaim);

                    if (result.Succeeded)
                    {
                        await GetUsers();
                        _state.SelectedUser = await GetUser(_state.SelectedUser.Id);

                        await _state.UsersGrid.Reload();
                        ShowNotification(NotificationSeverity.Success, "Done!", $"Claim: {newClaim.Value} has been added!", 5000);
                        BroadcastStateChange();
                    }
                    else
                    {
                        throw new Exception("Issue when adding claim!");
                    }
                }
                catch (Exception ex)
                {
                    StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                    ShowErrorPage(ex);
                }
            }
            else
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"No claim was chosen!", 5000);
            }
        }

        private async Task LockUser(bool input)
        {
            try
            {
                if (_state.SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
                {
                    _state.Lock = false;
                    ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be locked!", 5000);
                    BroadcastStateChange();
                    return;
                }

                var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);

                if (input)
                {
                    if (_state.SelectedUser.LockoutEnd != null) return;

                    var result = await UserManager.SetLockoutEndDateAsync(userIdentity, DateTime.Now.AddYears(+25));

                    if (result.Succeeded)
                    {
                        ShowNotification(NotificationSeverity.Warning, "Done!", $"User: {_state.SelectedUser.Name} has been locked!", 5000);
                        await GetUsers();
                        await _state.UsersGrid.Reload();
                    }
                }
                else
                {
                    if (_state.SelectedUser.LockoutEnd == null) return;

                    var result = await UserManager.SetLockoutEndDateAsync(userIdentity, null);

                    if (result.Succeeded)
                    {
                        ShowNotification(NotificationSeverity.Info, "Done!", $"User: {_state.SelectedUser.Name} has been unlocked!", 5000);
                        await GetUsers();
                        await _state.UsersGrid.Reload();
                    }
                }

                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        private async Task DeleteUser(UserModel user)
        {
            if (_state.SelectedUser.Claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
            {
                _state.SelectedRole = 0;
                ShowNotification(NotificationSeverity.Info, "Remember!", "Creator cannot be deleted!", 5000);
                return;
            }

            try
            {
                await UserManager.DeleteAsync(await UserManager.FindByIdAsync(user.Id));
                _state.Users.RemoveAll(x => x.Id == user.Id);

                await _state.UsersGrid.Reload();
                _state.SelectedUser = null;

                ShowNotification(NotificationSeverity.Warning, "Done!", $"User: {_state.SelectedUser.Name} has been deleted!", 5000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        #endregion

        public override void CleanUpStore()
        {
            throw new NotImplementedException();
        }

        public override void RefreshSore()
        {

        }
    }
}
