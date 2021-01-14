using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using SWP.Application.LegalSwp.AppDataAccess;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
using SWP.Domain.Enums;
using SWP.Domain.Models.LegalApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions;
using SWP.UI.Components.LegalSwpBlazorComponents.Dialogs;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Models;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager
{
    public class UserManagerState
    {
        public IEnumerable<int> SelectedClients;
        public IEnumerable<int> SelectedCases;

        public AppActiveUserManager.RelatedUser SelectedUser { get; set; }
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Case> Cases { get; set; } = new List<Case>();

        public bool IsStatisticsVisible { get; set; }
        public bool IsClientJobsVisible { get; set; }
        public bool IsClientContactsVisible { get; set; }
        public bool IsFinanceVisible { get; set; }
        public bool IsProductivityVisible { get; set; }
        public bool IsArchiveVisible { get; set; }

        public bool CanArchive { get; set; }
        public bool CanDelete { get; set; }
    }

    [UIScopedService]
    public class UserManagerStore : StoreBase<UserManagerState>
    {
        private readonly GeneralViewModel _generalViewModel;
        private readonly ILogger<UserManagerStore> _logger;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public UserManagerStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService, GeneralViewModel generalViewModel, ILogger<UserManagerStore> logger)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _generalViewModel = generalViewModel;
            _logger = logger;
        }

        private void UpdateData()
        {
            using var scope = _serviceProvider.CreateScope();
            var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

            _state.Clients = getClients.GetClientsWithCleanCases(MainStore.GetState().AppActiveUserManager.ProfileName);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SelectedUserChangeAction.SelectedUserChange:
                    var selectedUserChangeAction = (SelectedUserChangeAction)action;
                    await SelectedUserChange(selectedUserChangeAction.Arg);
                    break;
                case RemoveRelationAction.RemoveRelation:
                    await RemoveRelation();
                    break;
                case ConfirmRemoveAllDataAction.ConfirmRemoveAllData:
                    ConfirmRemoveAllData();
                    break;
                case UserManagerUpdateAccessAction.UserManagerUpdateAccess:
                    await UpdateAccess();
                    break;
                case UserManagerSelectedClientChangeAction.UserManagerSelectedClientChange:
                    var userManagerSelectedClientChangeAction = (UserManagerSelectedClientChangeAction)action;
                    SelectedClientChange(userManagerSelectedClientChangeAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private async Task SelectedUserChange(object value)
        {
            var input = (string)value;
            if (value != null)
            {
                _state.SelectedUser = MainStore.GetState().AppActiveUserManager.RelatedUsers.FirstOrDefault(x => x.Id == input);
                UpdateData();
                await GetCurrentAccesses();
            }
            else
            {
                _state.SelectedUser = null;
            }

            BroadcastStateChange();
        }

        private void SelectedClientChange(object arg)
        {
            var enumerable = arg as IEnumerable<int>;
            var selectedClients = enumerable.ToList();

            var availableCases = _state.Clients.Where(x => selectedClients.Contains(x.Id)).SelectMany(x => x.Cases).ToList();

            var casesToAdd = availableCases.Where(x => !_state.Cases.Any(y => y.Id.Equals(x.Id))).ToList();
            var casesToRemove = _state.Cases.Where(x => !availableCases.Any(y => y.Id.Equals(x.Id))).ToList();

            _state.Cases.RemoveAll(x => casesToRemove.Any(y => y.Id.Equals(x.Id)));
            _state.Cases.AddRange(casesToAdd);

            if (_state.SelectedCases is not null)
            {
                var selectedItems = _state.SelectedCases.ToList();
                selectedItems.RemoveAll(x => !_state.Cases.Any(y => y.Id.Equals(x)));
                _state.SelectedCases = selectedItems;
            }

            BroadcastStateChange();
        }

        public void ClearSelections()
        {
            _state.SelectedUser = null;
            _state.SelectedCases = null;
            _state.SelectedClients = null;
        }

        public async Task UpdateAccess()
        {
            if (string.IsNullOrEmpty(_state.SelectedUser.Id))
            {
                return;
            }

            try
            {
                EnableLoading("Przetwarzanie zmian...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var getAccess = scope.ServiceProvider.GetRequiredService<GetAccess>();
                    var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
                    var grantAccess = scope.ServiceProvider.GetRequiredService<GrantAccess>();
                    var revokeAccess = scope.ServiceProvider.GetRequiredService<RevokeAccess>();

                    //var lockResult = await userManager.UpdateSecurityStampAsync(userToModify);

                    //if (!lockResult.Succeeded)
                    //{
                    //    var identityErrors = String.Join("; ", lockResult.Errors.Select(x => x.Description).ToList());
                    //    _logger.LogError(LogTags.LegalAppLogPrefix + "Issue when Locking User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, userToModify.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                    //    throw new Exception(identityErrors);
                    //}

                    var currentProfileClients = getClients.GetClientsWithCleanCases(MainStore.GetState().AppActiveUserManager.ProfileName);
                    var currentProfileCases = currentProfileClients.SelectMany(x => x.Cases).Select(x => x.Id).ToList();

                    var currentClientAccesses = await getAccess.GetAccessToClient(_state.SelectedUser.Id);
                    var currentCaseAccesses = await getAccess.GetAccessToCase(_state.SelectedUser.Id);

                    var selectedClientAccesses = _state.SelectedClients.ToList();
                    var selectedCasesAccesses = _state.SelectedCases.ToList();

                    //Client accesses

                    var clientAccessesToRemove = new List<int>();
                    clientAccessesToRemove.AddRange(currentClientAccesses.Where(x => !selectedClientAccesses.Any(y => y.Equals(x.ClientId))).Select(x => x.ClientId).ToList());

                    var clientAccessesToAdd = new List<int>();
                    clientAccessesToAdd.AddRange(selectedClientAccesses.Where(x => !currentClientAccesses.Any(y => y.ClientId.Equals(x)) && currentProfileClients.Any(y => y.Id.Equals(x))).ToList());

                    await revokeAccess.RevokeAccessToClients(_state.SelectedUser.Id, clientAccessesToRemove);
                    await grantAccess.GrantAccessToClients(_state.SelectedUser.Id, clientAccessesToAdd);

                    //Case accesses

                    var caseAccessesToRemove = new List<int>();
                    caseAccessesToRemove.AddRange(currentCaseAccesses.Where(x => !selectedCasesAccesses.Any(y => y.Equals(x.CaseId))).Select(x => x.CaseId).ToList());

                    var caseAccessesToAdd = new List<int>();
                    caseAccessesToAdd.AddRange(selectedCasesAccesses.Where(x => !currentCaseAccesses.Any(y => y.CaseId.Equals(x)) && currentProfileCases.Any(y => y.Equals(x))).ToList());

                    await revokeAccess.RevokeAccessToCases(_state.SelectedUser.Id, caseAccessesToRemove);
                    await grantAccess.GrantAccessToCases(_state.SelectedUser.Id, caseAccessesToAdd);

                    //Panels accesses

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var user = await userManager.FindByIdAsync(_state.SelectedUser.Id);
                    var claims = await userManager.GetClaimsAsync(user) as List<Claim>;

                    if (!_state.IsStatisticsVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.MyApp.ToString())))
                    {
                        if (_state.IsStatisticsVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.MyApp.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.MyApp.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsStatisticsVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.MyApp.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.MyApp.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.IsClientJobsVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientJobs.ToString())))
                    {
                        if (_state.IsClientJobsVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientJobs.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.ClientJobs.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsClientJobsVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientJobs.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.ClientJobs.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.IsClientContactsVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientDetails.ToString())))
                    {
                        if (_state.IsClientContactsVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientDetails.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.ClientDetails.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsClientContactsVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.ClientDetails.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.ClientDetails.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.IsFinanceVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Finance.ToString())))
                    {
                        if (_state.IsFinanceVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Finance.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.Finance.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsFinanceVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Finance.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.Finance.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.IsProductivityVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Productivity.ToString())))
                    {
                        if (_state.IsProductivityVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Productivity.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.Productivity.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsProductivityVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Productivity.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.Productivity.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.IsArchiveVisible.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Archive.ToString())))
                    {
                        if (_state.IsArchiveVisible && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Archive.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppPanels.Archive.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.IsArchiveVisible && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppPanels.Archive.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppPanels.Archive.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.CanArchive.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanArchive.ToString())))
                    {
                        if (_state.CanArchive && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanArchive.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppActions.CanArchive.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.CanArchive && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanArchive.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppActions.CanArchive.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }

                    if (!_state.CanArchive.Equals(claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanDelete.ToString())))
                    {
                        if (_state.CanArchive && !claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanDelete.ToString()))
                        {
                            var newClaim = new Claim(ApplicationType.LegalApplication.ToString(), LegalAppActions.CanDelete.ToString());
                            var result = await userManager.AddClaimAsync(user, newClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                        else if (!_state.CanArchive && claims.Any(x => x.Type == ApplicationType.LegalApplication.ToString() && x.Value == LegalAppActions.CanDelete.ToString()))
                        {
                            var deletedClaim = claims.FirstOrDefault(x => x.Type.Equals(ApplicationType.LegalApplication.ToString()) && x.Value.Equals(LegalAppActions.CanDelete.ToString()));
                            var result = await userManager.RemoveClaimAsync(user, deletedClaim);

                            if (!result.Succeeded)
                            {
                                var identityErrors = string.Join("; ", result.Errors.Select(x => x.Description).ToList());
                                _logger.LogError(LogTags.LegalAppLogPrefix + "Issue during access modification for Profile {profileRemove} for User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, MainStore.GetState().AppActiveUserManager.ProfileName, user.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                                throw new Exception(identityErrors);
                            }
                        }
                    }
                }

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zmieniono dostępy dla: {_state.SelectedUser.UserName}.", GeneralViewModel.NotificationDuration);
                await GetCurrentAccesses();
                DisableLoading();
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogTags.LegalAppLogPrefix + "Exception in UpdateAccess method in Legal App Options panel, User: {rootUser}", MainStore.GetState().AppActiveUserManager.UserName);
                DisableLoading();
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task RemoveRelation()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var profileClaim = MainStore.GetState().AppActiveUserManager.ProfileClaim;
                var selectedUser = MainStore.GetState().AppActiveUserManager.RelatedUsers.FirstOrDefault(x => x.Id == _state.SelectedUser.Id);
                var userToRemove = await userManager.FindByIdAsync(_state.SelectedUser.Id);

                var lockResult = await userManager.UpdateSecurityStampAsync(userToRemove);

                if (!lockResult.Succeeded)
                {
                    var identityErrors = String.Join("; ", lockResult.Errors.Select(x => x.Description).ToList());
                    _logger.LogError(LogTags.LegalAppLogPrefix + "Issue when Locking User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, userToRemove.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                    throw new Exception(identityErrors);
                }

                var profileRemoveResult = await userManager.RemoveClaimAsync(userToRemove, profileClaim);

                if (!profileRemoveResult.Succeeded)
                {
                    var identityErrors = String.Join("; ", lockResult.Errors.Select(x => x.Description).ToList());
                    _logger.LogError(LogTags.LegalAppLogPrefix + "Issue when removing Profile {profileRemove} from User: {lockUser} by User: {rootUser} - Errors:" + identityErrors, profileClaim.Value, userToRemove.UserName, MainStore.GetState().AppActiveUserManager.UserName);
                    throw new Exception(identityErrors);
                }

                _state.SelectedUser = null;
                await MainStore.RealodUserData();

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Użytkownik: {userToRemove.UserName} został usunięty z profilu {profileClaim.Value}.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogTags.LegalAppLogPrefix + "Exception in RemoveRelation method in Legal App Options panel, User: {rootUser}", MainStore.GetState().AppActiveUserManager.UserName);
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task RemoveProfileData()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteClient = scope.ServiceProvider.GetRequiredService<DeleteClient>();

                await deleteClient.Delete(MainStore.GetState().AppActiveUserManager.ProfileName);

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Usunięto wszystkie dane powiązane z profilem: {MainStore.GetState().AppActiveUserManager.ProfileName}", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ConfirmRemoveAllData()
        {
            EnableLoading("Usuwanie Danych...");

            _dialogService.Open<GenericDialogPopup>("Uwaga! Usunięcie danych będzie nieodwracalne!",
                new Dictionary<string, object>()
                {
                    { "Title", "Potwierdź usunięcie danych" },
                    { "TaskToExecuteAsync", new Func<Task>(RemoveProfileData) },
                    { "Description", "This is sample Description" },
                },
                _generalViewModel.DefaultDialogOptions);

            DisableLoading();
        }

        private async Task GetCurrentAccesses()
        {
            //var getAccess = scope.ServiceProvider.GetRequiredService<GetAccess>();
            //var grantAccess = scope.ServiceProvider.GetRequiredService<GrantAccess>();
            //var revokeAccess = scope.ServiceProvider.GetRequiredService<RevokeAccess>();

            if (_state.SelectedUser is null) return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var user = new AppActiveUserManager(_serviceProvider, _state.SelectedUser.Id);

                await user.UpdateClaimsAndRoles();
                await user.UpdatePermissions();
                //await user.UpdateLicenses();

                //var currentCasesAccess = await getAccess.GetAccessToCase(_state.SelectedUser.Id);
                //var currentClientsAccess = await getAccess.GetAccessToClient(_state.SelectedUser.Id);

                _state.SelectedClients = user.ClientsPermissions;
                _state.SelectedCases = user.CasesPermissions;

                //Update selected content in listboxes
                var selectedClients = _state.SelectedClients.ToList();
                var availableCases = _state.Clients.Where(x => selectedClients.Contains(x.Id)).SelectMany(x => x.Cases).ToList();

                var casesToAdd = availableCases.Where(x => !_state.Cases.Any(y => y.Id.Equals(x.Id))).ToList();
                var casesToRemove = _state.Cases.Where(x => !availableCases.Any(y => y.Id.Equals(x.Id))).ToList();

                _state.Cases.RemoveAll(x => casesToRemove.Any(y => y.Id.Equals(x.Id)));
                _state.Cases.AddRange(casesToAdd);

                if (_state.SelectedCases is not null)
                {
                    var selectedItems = _state.SelectedCases.ToList();
                    selectedItems.RemoveAll(x => !_state.Cases.Any(y => y.Id.Equals(x)));
                    _state.SelectedCases = selectedItems;
                }

                _state.IsStatisticsVisible = user.IsStatisticsVisible;
                _state.IsClientJobsVisible = user.IsClientJobsVisible;
                _state.IsClientContactsVisible = user.IsClientContactsVisible;
                _state.IsFinanceVisible = user.IsFinanceVisible;
                _state.IsProductivityVisible = user.IsProductivityVisible;
                _state.IsArchiveVisible = user.IsArchiveVisible;
                _state.CanArchive = user.CanArchive;
                _state.CanDelete = user.CanDelete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogTags.LegalAppLogPrefix + "Exception in GetCurrentAccesses method in Legal App Options panel, User: {rootUser}", MainStore.GetState().AppActiveUserManager.UserName);
                MainStore.ShowErrorPage(ex);
            }
        }
    }
}
