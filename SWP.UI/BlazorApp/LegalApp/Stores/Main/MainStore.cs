using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.Log;
using SWP.Domain.Models.SWPLegal;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Error;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Main
{
    public class MainState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public string ActiveUserId { get; set; }
        public UserModel User { get; set; } = new UserModel();
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();
        public ClientViewModel ActiveClient { get; set; }
        public LegalAppPanels ActivePanel { get; set; } = LegalAppPanels.MyApp;
        public string SelectedClientString { get; set; }
    }

    [UIScopedService]
    public class MainStore : StoreBase
    {
        private readonly MainState _state;
        private readonly ErrorStore _errorStore;

        public MainState GetState() => _state;

        public MainStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, ErrorStore errorStore) : base(serviceProvider, actionDispatcher, notificationService)
        {
            _state = new MainState();
            _errorStore = errorStore;
        }

        public async Task InitializeState(string userId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                _state.ActiveUserId = userId;

                _state.User.User = await userManager.FindByIdAsync(_state.ActiveUserId);
                _state.User.Claims = await userManager.GetClaimsAsync(_state.User.User) as List<Claim>;
                _state.User.Roles = await userManager.GetRolesAsync(_state.User.User) as List<string>;
                _state.User.RelatedUsers = await userManager.GetUsersForClaimAsync(_state.User.ProfileClaim);

                _state.Clients = getClients.GetClientsWithoutData(_state.User.Profile)?
                    .Select(x => (ClientViewModel)x).ToList();
            }
            catch (Exception ex)
            {
                using var scope = _serviceProvider.CreateScope();
                var createLogRecord = scope.ServiceProvider.GetRequiredService<CreateLogRecord>();

                await createLogRecord.Create(new CreateLogRecord.Request
                {
                    Message = ex.Message,
                    UserId = _state.ActiveUserId,
                    StackTrace = ex.StackTrace
                });
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SetActivePanel(LegalAppPanels panel) => _state.ActivePanel = panel;

        public void ActivateLoading(string message)
        {
            _state.LoadingMessage = message;
            _state.Loading = true;
        }

        public void DeactivateLoading()
        {
            _state.Loading = false;
            BroadcastStateChange();
        }

        public async Task ActiveClientChange(object value)
        {
            if (_state.Loading)
            {
                return;
            }
            else
            {
                ActivateLoading("Wczytywanie Klienta...");
            }

            if (value != null)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                    _state.ActiveClient = getClient.Get(int.Parse(value.ToString()));

                    //FinancePage.GetDataForMonthFilter();
                    //ProductivityPage.GetDataForMonthFilter();

                    DeactivateLoading();
                }
                catch (Exception ex)
                {
                    await ShowErrorPage(ex);
                }
                finally
                {
                    DeactivateLoading();
                }
            }
            else
            {
                if (_state.ActivePanel != LegalAppPanels.Calendar)
                {
                    SetActivePanel(LegalAppPanels.MyApp);
                }

                _state.ActiveClient = null;

                DeactivateLoading();
            }
        }

        public async Task ShowErrorPage(Exception ex)
        {
            await _errorStore.SetException(ex, _state.ActiveUserId);
            _state.ActivePanel = LegalAppPanels.ErrorPage;
            BroadcastStateChange();
        }

        public void DismissErrorPage()
        {
            _state.ActivePanel = LegalAppPanels.MyApp;
            BroadcastStateChange();
        }

        public async Task RefreshRelatedUsers()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                _state.User.RelatedUsers = await userManager.GetUsersForClaimAsync(_state.User.ProfileClaim);
            }
            catch (Exception ex)
            {
                await ShowErrorPage(ex);
            }
        }

        public Task ThrowTestException()
        {
            try
            {
                throw new Exception("bablabla");
            }
            catch (Exception ex)
            {
                return ShowErrorPage(ex);
            }
        }

        public void UpdateClientsList(ClientViewModel input) => _state.Clients[_state.Clients.FindIndex(x => x.Id == input.Id)] = input;

        public void UpdateClientContactPerson(ClientContactPerson input) => _state.ActiveClient.ContactPeople[_state.ActiveClient.ContactPeople.FindIndex(x => x.Id == input.Id)] = input;

        public void RemoveClientContactPerson(int id) => _state.ActiveClient.ContactPeople.RemoveAll(x => x.Id == id);

        public void AddClientContactPerson(ClientContactPerson input) => _state.ActiveClient.ContactPeople.Add(input);

        public void RefreshClients()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getClients = scope.ServiceProvider.GetRequiredService<GetClients>();

                _state.Clients = getClients.GetClientsWithoutData(_state.User.Profile, true).Select(x => (ClientViewModel)x).ToList();
                _state.ActiveClient = null;
                _state.SelectedClientString = null;
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void AddClient(ClientViewModel input) => _state.Clients.Add(input);

        public void RemoveClient(int id)
        {
            _state.Clients.RemoveAll(x => x.Id == id);

            if (_state.ActiveClient?.Id == id)
            {
                _state.ActiveClient = null;
                _state.SelectedClientString = null;
            }

            BroadcastStateChange();
        }

        public ClientViewModel GetActiveClient() => _state.ActiveClient;

        public UserModel GetApplicationUser() => _state.User;

        public void RefreshActiveClientData()
        {
            try
            {
                if (_state.ActiveClient != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var getClient = scope.ServiceProvider.GetRequiredService<GetClient>();

                    ClientViewModel newModel = getClient.Get(_state.ActiveClient.Id);

                    if (_state.ActiveClient.SelectedCase != null)
                    {
                        newModel.SelectedCase = newModel.Cases.FirstOrDefault(x => x.Id == _state.ActiveClient.SelectedCase.Id);
                    }

                    _state.ActiveClient = newModel;
                }
            }
            catch (Exception ex)
            {
                ShowErrorPage(ex).GetAwaiter();
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void ReloadCase(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            var caseEntity = getCase.Get(id);
            _state.ActiveClient.Cases.RemoveAll(x => x.Id == id);
            _state.ActiveClient.Cases.Add(caseEntity);
            _state.ActiveClient.Cases = _state.ActiveClient.Cases.OrderBy(x => x.Name).ToList();
            _state.ActiveClient.Cases.TrimExcess();
            _state.ActiveClient.SelectedCase = caseEntity;
        }

        public void ClearSelectedCase() => _state.ActiveClient.SelectedCase = null;

        public void AddCaseToActiveClient(CaseViewModel entity) => _state.ActiveClient.Cases.Add(entity);

        public void RemoveCaseFromActiveClient(int id) => _state.ActiveClient.Cases.RemoveAll(x => x.Id == id);

        public void ReplaceCaseFromActiveClient(CaseViewModel entity) => _state.ActiveClient.Cases[_state.ActiveClient.Cases.FindIndex(x => x.Id == entity.Id)] = entity;

        public void SetSelectedCase(CaseViewModel entity) => _state.ActiveClient.SelectedCase = entity;

        public void SetSelectedNote(NoteViewModel entity) => _state.ActiveClient.SelectedCase.SelectedNote = entity;

        public void AddNoteToActiveCase(NoteViewModel entity) => _state.ActiveClient.SelectedCase.Notes.Add(entity);

        public void RemoveNoteFromActiveCase(int id) => _state.ActiveClient.SelectedCase.Notes.RemoveAll(x => x.Id == id);

        public void ReplaceNoteFromActiveCase(NoteViewModel entity) => _state.ActiveClient.SelectedCase.Notes[_state.ActiveClient.SelectedCase.Notes.FindIndex(x => x.Id == entity.Id)] = entity;

        public void AddReminderToActiveCase(ReminderViewModel entity) => _state.ActiveClient.SelectedCase.Reminders.Add(entity);

        public void RemoveReminderFromActiveCase(int id) => _state.ActiveClient.SelectedCase.Reminders.RemoveAll(x => x.Id == id);

        public void ReplaceReminderFromActiveCase(ReminderViewModel entity) => _state.ActiveClient.SelectedCase.Reminders[_state.ActiveClient.SelectedCase.Reminders.FindIndex(x => x.Id == entity.Id)] = entity;

        public void AddContactToActiveCase(ContactPersonViewModel entity) => _state.ActiveClient.SelectedCase.ContactPeople.Add(entity);

        public void RemoveContactFromActiveCase(int id) => _state.ActiveClient.SelectedCase.ContactPeople.RemoveAll(x => x.Id == id);

        public void ReplaceContactFromActiveCase(ContactPersonViewModel entity) => _state.ActiveClient.SelectedCase.ContactPeople[_state.ActiveClient.SelectedCase.ContactPeople.FindIndex(x => x.Id == entity.Id)] = entity;

        public void AddCashMovementToActiveClient(CashMovementViewModel entity) => _state.ActiveClient.CashMovements.Add(entity);

        public void RemoveCashMovementFromActiveClient(int id) => _state.ActiveClient.CashMovements.RemoveAll(x => x.Id == id);

        public void ReplaceCashMovementFromActiveClient(CashMovementViewModel entity) => _state.ActiveClient.CashMovements[_state.ActiveClient.CashMovements.FindIndex(x => x.Id == entity.Id)] = entity;

        public void SetSelectedCashMovement(CashMovementViewModel entity) => _state.ActiveClient.SelectedCashMovement = entity;

        public void AddTimeRecordToActiveClient(TimeRecordViewModel entity) => _state.ActiveClient.TimeRecords.Add(entity);

        public void RemoveTimeRecordFromActiveClient(int id) => _state.ActiveClient.CashMovements.RemoveAll(x => x.Id == id);

        public void ReplaceTimeRecordFromActiveClient(TimeRecordViewModel entity) => _state.ActiveClient.TimeRecords[_state.ActiveClient.TimeRecords.FindIndex(x => x.Id == entity.Id)] = entity;

        public void SetSelectedTimeRecord(TimeRecordViewModel entity) => _state.ActiveClient.SelectedTimeRecord = entity;

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }
    }
}
