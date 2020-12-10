using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
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
        public string ProfileClaimNameFromList { get; set; } = "";
        public RadzenGrid<Claim> ClaimsGrid { get; set; }
        public RadzenGrid<UserModel> UsersGrid { get; set; }
        public RadzenGrid<UserLicense> LicensesGrid { get; set; }
        public List<string> StatusClaims => Enum.GetNames(typeof(UserStatus)).ToList();
        public List<string> ApplicationClaims => Enum.GetNames(typeof(ApplicationType)).ToList();
        public List<string> LicenseTypeClaims => Enum.GetNames(typeof(LicenseType)).ToList();
        public List<string> Roles => Enum.GetNames(typeof(RoleType)).ToList();
        public bool Loading { get; set; }
        public UserModel SelectedUser { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public List<ProfileModel> AllProfiles { get; set; } = new List<ProfileModel>();
        public CreateLicense.Request NewLicense { get; set; } = new CreateLicense.Request();
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
        public List<UserLicense> Licenses { get; set; }
    }

    public class ProfileModel
    {
        public string Id { get; set; }
        public string ProfileName { get; set; }
    }

    [UIScopedService]
    public class UsersStore : StoreBase<UserState>
    {
        private readonly IPortalManager _portalManager;

        UserManager<IdentityUser> UserManager => _serviceProvider.GetService<UserManager<IdentityUser>>();
        ApplicationStore AppStore => _serviceProvider.GetRequiredService<ApplicationStore>();
        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();

        public UsersStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, IPortalManager portalManager)
            : base(serviceProvider, actionDispatcher, notificationService)
        {
            _portalManager = portalManager;
        }

        public async Task Initialize()
        {
            await GetUsers();
            _state.SelectedRole = _state.SelectedUser.UserRoleInt;
            RefreshSore();

            BroadcastStateChange();
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case UserRowSelectedAction.UserRowSelected:
                    var rowSelectedAction = (UserRowSelectedAction)action;
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
                case AddProfileClaimFromListAction.AddProfileClaimFromList:
                    await AddProfileClaimFromList();
                    break;
                case OnUpdateLicenseRowAction.OnUpdateLicenseRow:
                    var onUpdateLicenseRowAction = (OnUpdateLicenseRowAction)action;
                    await OnUpdateLicenseRow(onUpdateLicenseRowAction.Arg);
                    break;
                case SelectedProfileChangeAction.SelectedProfileChange:
                    var selectedProfileChangeAction = (SelectedProfileChangeAction)action;
                    SelectedProfileChange(selectedProfileChangeAction.Arg);
                    break;
                case LockUserAction.LockUser:
                    var lockUserAction = (LockUserAction)action;
                    await LockUser(lockUserAction.Arg);
                    break;
                case DeleteUserAction.DeleteUser:
                    var deleteUserAction = (DeleteUserAction)action;
                    await DeleteUser(deleteUserAction.Arg);
                    break;
                case DeleteLicenseRowAction.DeleteLicenseRow:
                    var deleteLicenseRowAction = (DeleteLicenseRowAction)action;
                    await DeleteLicenseRow(deleteLicenseRowAction.Arg);
                    break;
                case SubmitNewLicenseAction.SubmitNewLicense:
                    var submitNewLicenseAction = (SubmitNewLicenseAction)action;
                    await SubmitNewLicense(submitNewLicenseAction.Arg);
                    break;
                case EditLicenseRowAction.EditLicenseRow:
                    var editLicenseRowAction = (EditLicenseRowAction)action;
                    EditLicenseRow(editLicenseRowAction.Arg);
                    break;
                case SaveLicenseRowAction.SaveLicenseRow:
                    var saveLicenseRowAction = (SaveLicenseRowAction)action;
                    SaveLicenseRow(saveLicenseRowAction.Arg);
                    break;
                case CancelLicenseEditAction.CancelLicenseEdit:
                    var cancelLicenseEditAction = (CancelLicenseEditAction)action;
                    CancelLicenseEdit(cancelLicenseEditAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void SelectedProfileChange(object c) => _state.ProfileClaimNameFromList = _state.AllProfiles.Where(x => x.Id == c.ToString()).Select(x => x.ProfileName).FirstOrDefault();

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
                LockoutEnabled = user.LockoutEnabled,
                Licenses = GetUserLicenses(user.Id)
            };
        }

        private List<UserLicense> GetUserLicenses(string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var getLicense = scope.ServiceProvider.GetRequiredService<GetLicense>();

            return getLicense.GetAll(userId);
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

        private async Task AddProfileClaimFromList()
        {
            if (_state.SelectedUser.Claims.Any(x => x.Type == ClaimType.Profile.ToString()))
            {
                ShowNotification(NotificationSeverity.Error, "Remember!", $"User can have only one profile!", 5000);
                return;
            }

            if (!string.IsNullOrEmpty(_state.ProfileClaimNameFromList.Trim()))
            {
                try
                {
                    var userIdentity = await UserManager.FindByIdAsync(_state.SelectedUser.Id);
                    var newClaim = new Claim(ClaimType.Profile.ToString(), _state.ProfileClaimNameFromList.Trim());
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
                var name = _state.SelectedUser.Name;
                _state.SelectedUser = null;

                ShowNotification(NotificationSeverity.Warning, "Done!", $"User: {name} has been deleted!", 5000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }


        private async Task SubmitNewLicense(CreateLicense.Request request)
        {
            if (_state.SelectedUser.Licenses.Any(x => x.Application == request.Product))
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"User already have this License!", 2000);
                return;
            }

            if (!_state.SelectedUser.RootClient)
            {
                ShowNotification(NotificationSeverity.Error, "Error!", $"Only Root Clients can have licenses!", 2000);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createLicense = scope.ServiceProvider.GetRequiredService<CreateLicense>();

                request.UserId = _state.SelectedUser.Id;
                request.CreatedBy = AppStore.GetState().User.UserName;

                var result = await createLicense.Create(request);
                _state.NewLicense = new CreateLicense.Request();

                _state.SelectedUser.Licenses.Add(result);
                await _state.LicensesGrid.Reload();

                ShowNotification(NotificationSeverity.Success, "Done!", $"Status Claim: {_state.SelectedStatusClaim} has been added!", 2000);
                BroadcastStateChange();

            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        private void EditLicenseRow(UserLicense userLicense) => _state.LicensesGrid.EditRow(userLicense);

        private void SaveLicenseRow(UserLicense userLicense) => _state.LicensesGrid.UpdateRow(userLicense);

        private void CancelLicenseEdit(UserLicense userLicense)
        {
            _state.LicensesGrid.CancelEditRow(userLicense);
            _state.SelectedUser.Licenses = GetUserLicenses(_state.SelectedUser.Id);
            BroadcastStateChange();
        }

        private async Task OnUpdateLicenseRow(UserLicense userLicense)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateLicense = scope.ServiceProvider.GetRequiredService<UpdateLicense>();

                userLicense.Updated = DateTime.Now;
                userLicense.UpdatedBy = AppStore.GetState().User.UserName;

                var result = await updateLicense.Update(userLicense);

                _state.SelectedUser.Licenses[_state.SelectedUser.Licenses.FindIndex(x => x.Id == result.Id)] = result;
                await _state.LicensesGrid.Reload();

                ShowNotification(NotificationSeverity.Success, "Success!", $"License: {result.Application}, for User: {_state.SelectedUser.Name} has been changed.", 2000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex);
            }
        }

        private async Task DeleteLicenseRow(UserLicense userLicense)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteLicense = scope.ServiceProvider.GetRequiredService<DeleteLicense>();

                await deleteLicense.Delete(userLicense.Id);

                _state.SelectedUser.Licenses.RemoveAll(x => x.Id == userLicense.Id);
                await _state.LicensesGrid.Reload();

                ShowNotification(NotificationSeverity.Warning, "Success!", $"License: {userLicense.Application} - {userLicense.Type} has been removed.", 2000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
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
            var profiles = _portalManager.GetAllProfiles();
            _state.AllProfiles = profiles.Select(x => new ProfileModel { Id = profiles.IndexOf(x).ToString(), ProfileName = x }).ToList();
        }
    }
}
