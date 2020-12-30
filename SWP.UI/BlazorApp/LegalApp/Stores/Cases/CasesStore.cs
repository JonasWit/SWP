using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.Application.LegalSwp.Notes;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases
{
    public class CasesState : StateBase
    {
        public CreateCase.Request NewCase { get; set; } = new CreateCase.Request();
        public CreateNote.Request NewNote { get; set; } = new CreateNote.Request();
        public CreateContactPerson.Request NewContact { get; set; } = new CreateContactPerson.Request();
        public RadzenGrid<CaseViewModel> CasesGrid { get; set; }
        public RadzenGrid<NoteViewModel> NotesGrid { get; set; }
        public RadzenGrid<ContactPersonViewModel> ContactsGrid { get; set; }
        public RadzenScheduler<ReminderViewModel> CasesScheduler { get; set; }
        public ContactPersonViewModel SelectedContact { get; set; }
        public NoteViewModel SelectedNote { get; set; }
        public CaseViewModel SelectedCase { get; set; }
        public List<CaseViewModel> Cases { get; set; } = new List<CaseViewModel>();
    }

    [UIScopedService]
    public class CasesStore : StoreBase<CasesState>
    {
        private readonly GeneralViewModel _generalViewModel;

        private MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public CasesStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService, GeneralViewModel generalViewModel)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _generalViewModel = generalViewModel;
        }

        public void Initialize()
        {
            GetCases(MainStore.GetState().ActiveClient.Id);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case CreateNewCaseAction.CreateNewCase:
                    var createNewCaseAction = (CreateNewCaseAction)action;
                    await CreateNewCase(createNewCaseAction.Request);
                    break;
                case EditCaseRowAction.EditCaseRow:
                    var editCaseRowAction = (EditCaseRowAction)action;
                    EditCaseRow(editCaseRowAction.Arg);
                    break;
                case OnUpdateCaseRowAction.OnUpdateCaseRow:
                    var onUpdateCaseRowAction = (OnUpdateCaseRowAction)action;
                    await OnUpdateCaseRow(onUpdateCaseRowAction.Arg);
                    break;
                case CancelEditCaseRowAction.CancelEditCaseRow:
                    var cancelEditCaseRowAction = (CancelEditCaseRowAction)action;
                    CancelEditCaseRow(cancelEditCaseRowAction.Arg);
                    break;
                case SaveCaseRowAction.SaveCaseRow:
                    var saveCaseRowAction = (SaveCaseRowAction)action;
                    SaveCaseRow(saveCaseRowAction.Arg);
                    break;
                case DeleteCaseRowAction.DeleteCaseRow:
                    var deleteCaseRowAction = (DeleteCaseRowAction)action;
                    await DeleteCaseRow(deleteCaseRowAction.Arg);
                    break;
                case ActiveCaseChangeAction.ActiveCaseChange:
                    var activeCaseChangeAction = (ActiveCaseChangeAction)action;
                    ActiveCaseChange(activeCaseChangeAction.Arg);
                    break;
                case ArchivizeCaseAction.ArchivizeCase:
                    var archivizeCaseAction = (ArchivizeCaseAction)action;
                    await ArchivizeCase(archivizeCaseAction.Arg);
                    break;
                case ActiveNoteChangeAction.ActiveNoteChange:
                    var activeNoteChangeAction = (ActiveNoteChangeAction)action;
                    ActiveNoteChange(activeNoteChangeAction.Arg);
                    break;
                case CreateNewNoteAction.CreateNewNote:
                    var createNewNoteAction = (CreateNewNoteAction)action;
                    await CreateNewNote(createNewNoteAction.Request);
                    break;
                case EditNoteRowAction.EditNoteRow:
                    var editNoteRowAction = (EditNoteRowAction)action;
                    EditNoteRow(editNoteRowAction.Arg);
                    break;
                case OnUpdateNoteRowAction.OnUpdateNoteRow:
                    var onUpdateNoteRowAction = (OnUpdateNoteRowAction)action;
                    await OnUpdateNoteRow(onUpdateNoteRowAction.Arg);
                    break;
                case SaveNoteRowAction.SaveNoteRow:
                    var saveNoteRowAction = (SaveNoteRowAction)action;
                    SaveNoteRow(saveNoteRowAction.Arg);
                    break;
                case CancelEditNoteRowAction.CancelEditNoteRow:
                    var cancelEditNoteRowAction = (CancelEditNoteRowAction)action;
                    CancelEditNoteRow(cancelEditNoteRowAction.Arg);
                    break;
                case DeleteNoteRowAction.DeleteNoteRow:
                    var deleteNoteRowAction = (DeleteNoteRowAction)action;
                    await DeleteNoteRow(deleteNoteRowAction.Arg);
                    break;
                case OnSlotSelectAction.OnSlotSelect:
                    var onSlotSelectAction = (OnSlotSelectAction)action;
                    await OnSlotSelect(onSlotSelectAction.Arg);
                    break;
                case OnAppointmentSelectAction.OnAppointmentSelect:
                    var onAppointmentSelectAction = (OnAppointmentSelectAction)action;
                    await OnAppointmentSelect(onAppointmentSelectAction.Arg);
                    break;
                case OnAppointmentRenderAction.OnAppointmentRender:
                    var onAppointmentRenderAction = (OnAppointmentRenderAction)action;
                    OnAppointmentRender(onAppointmentRenderAction.Arg);
                    break;
                case OnUpdateContactRowAction.OnUpdateContactRow:
                    var onUpdateContactRowAction = (OnUpdateContactRowAction)action;
                    await OnUpdateContactRow(onUpdateContactRowAction.Arg);
                    break;
                case SaveContactRowAction.SaveContactRow:
                    var saveContactRowAction = (SaveContactRowAction)action;
                    SaveContactRow(saveContactRowAction.Arg);
                    break;
                case CancelContactEditAction.CancelContactEdit:
                    var cancelContactEditAction = (CancelContactEditAction)action;
                    CancelContactEdit(cancelContactEditAction.Arg);
                    break;
                case DeleteContactRowAction.DeleteContactRow:
                    var deleteContactRowAction = (DeleteContactRowAction)action;
                    await DeleteContactRow(deleteContactRowAction.Arg);
                    break;
                case SubmitNewContactAction.SubmitNewContactRow:
                    var submitNewContactAction = (SubmitNewContactAction)action;
                    await SubmitNewContact(submitNewContactAction.Request);
                    break;
                case ContactSelectedAction.ContactSelected:
                    var contactSelectedAction = (ContactSelectedAction)action;
                    ContactSelected(contactSelectedAction.Arg);
                    break;
                case EditContactRowAction.EditContactRow:
                    var editContactRowAction = (EditContactRowAction)action;
                    EditContactRow(editContactRowAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void GetCases(int clientId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getCases = scope.ServiceProvider.GetRequiredService<GetCases>();

                _state.Cases = getCases.GetCasesForClient(clientId).Where(x => x.Active).Select(x => (CaseViewModel)x).ToList();

                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ClearSelectedCase() => _state.SelectedCase = null;

        private void SetSelectedCase(CaseViewModel entity) => _state.SelectedCase = entity;

        private void SetSelectedNote(NoteViewModel entity) => _state.SelectedNote = entity;

        private void AddNoteToActiveCase(NoteViewModel entity) => _state.SelectedCase.Notes.Add(entity);

        private void RemoveNoteFromActiveCase(int id) => _state.SelectedCase.Notes.RemoveAll(x => x.Id == id);

        private void ReplaceNoteFromActiveCase(NoteViewModel entity) => _state.SelectedCase.Notes[_state.SelectedCase.Notes.FindIndex(x => x.Id == entity.Id)] = entity;

        private void AddReminderToActiveCase(ReminderViewModel entity) => _state.SelectedCase.Reminders.Add(entity);

        private void RemoveReminderFromActiveCase(int id) => _state.SelectedCase.Reminders.RemoveAll(x => x.Id == id);

        private void ReplaceReminderFromActiveCase(ReminderViewModel entity) => _state.SelectedCase.Reminders[_state.SelectedCase.Reminders.FindIndex(x => x.Id == entity.Id)] = entity;

        private void AddContactToActiveCase(ContactPersonViewModel entity) => _state.SelectedCase.ContactPeople.Add(entity);

        private void RemoveContactFromActiveCase(int id) => _state.SelectedCase.ContactPeople.RemoveAll(x => x.Id == id);

        private void ReplaceContactFromActiveCase(ContactPersonViewModel entity) => _state.SelectedCase.ContactPeople[_state.SelectedCase.ContactPeople.FindIndex(x => x.Id == entity.Id)] = entity;

        #region Cases Management

        private async Task CreateNewCase(CreateCase.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createCase = scope.ServiceProvider.GetRequiredService<CreateCase>();

                request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;
                var result = await createCase.Create(MainStore.GetState().ActiveClient.Id, MainStore.GetState().AppActiveUserManager.ProfileName, request);
                _state.NewCase = new CreateCase.Request();

                AddCaseToActiveClient(result);

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void EditCaseRow(CaseViewModel c) => _state.CasesGrid.EditRow(c);

        private async Task OnUpdateCaseRow(CaseViewModel c)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateCase = scope.ServiceProvider.GetRequiredService<UpdateCase>();

                var result = await updateCase.Update(new UpdateCase.Request
                {
                    Id = c.Id,
                    CaseType = c.CaseType,
                    Description = c.Description,
                    Name = c.Name,
                    Signature = c.Signature,
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                ReplaceCaseFromActiveClient(result);

                if (_state.SelectedCase.Id == result.Id)
                {
                    SetSelectedCase(_state.Cases.FirstOrDefault(x => x.Id == result.Id));
                }

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveCaseRow(CaseViewModel c) => _state.CasesGrid.UpdateRow(c);

        private void CancelEditCaseRow(CaseViewModel c)
        {
            _state.CasesGrid.CancelEditRow(c);
            MainStore.RefreshMainComponent();
            BroadcastStateChange();
        }

        private async Task DeleteCaseRow(CaseViewModel c)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteCase = scope.ServiceProvider.GetRequiredService<DeleteCase>();

                await deleteCase.Delete(c.Id);

                RemoveCaseFromActiveClient(c.Id);

                if (_state.SelectedCase != null &&
                    _state.SelectedCase.Id == c.Id)
                {
                    ClearSelectedCase();
                }

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Sprawa: {c.Name} została usunięta.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ActiveCaseChange(object value)
        {
            var input = (CaseViewModel)value;
            if (value != null)
            {
                SetSelectedCase(_state.Cases.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                SetSelectedCase(null);
            }

            _state.SelectedContact = null;
        }

        private async Task ArchivizeCase(CaseViewModel c)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveCase = scope.ServiceProvider.GetRequiredService<ArchiveCases>();

                _state.Cases.RemoveAll(x => x.Id == c.Id);
                await archiveCase.ArchivizeCase(c.Id, MainStore.GetState().AppActiveUserManager.UserName);

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {c.Name} została zarchwizowana", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ReloadCase(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            var caseEntity = getCase.Get(id);
            _state.Cases.RemoveAll(x => x.Id == id);
            _state.Cases.Add(caseEntity);
            _state.Cases = _state.Cases.OrderBy(x => x.Name).ToList();
            _state.Cases.TrimExcess();
        }

        private void AddCaseToActiveClient(CaseViewModel entity) => _state.Cases.Add(entity);

        private void RemoveCaseFromActiveClient(int id) => _state.Cases.RemoveAll(x => x.Id == id);

        private void ReplaceCaseFromActiveClient(CaseViewModel entity) => _state.Cases[_state.Cases.FindIndex(x => x.Id == entity.Id)] = entity;

        #endregion

        #region Notes Tab

        private void ActiveNoteChange(object value)
        {
            var input = (NoteViewModel)value;
            if (value != null)
            {
                SetSelectedNote(_state.SelectedCase.Notes.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                SetSelectedNote(null);
            }
        }

        private async Task CreateNewNote(CreateNote.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createNote = scope.ServiceProvider.GetRequiredService<CreateNote>();

                request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;
                var result = await createNote.Create(_state.SelectedCase.Id, request);

                _state.NewNote = new CreateNote.Request();

                AddNoteToActiveCase(result);

                await _state.NotesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void EditNoteRow(NoteViewModel note) => _state.NotesGrid.EditRow(note);

        private async Task OnUpdateNoteRow(NoteViewModel note)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateNote = scope.ServiceProvider.GetRequiredService<UpdateNote>();

                var result = await updateNote.Update(new UpdateNote.Request
                {
                    Id = note.Id,
                    Message = note.Message,
                    Name = note.Name,
                    Priority = note.Priority,
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                ReplaceNoteFromActiveCase(result);

                await _state.NotesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveNoteRow(NoteViewModel note) => _state.NotesGrid.UpdateRow(note);

        private void CancelEditNoteRow(NoteViewModel note)
        {
            _state.NotesGrid.CancelEditRow(note);
            ReloadCase(note.CaseId);
            BroadcastStateChange();
        }

        private async Task DeleteNoteRow(NoteViewModel note)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteNote = scope.ServiceProvider.GetRequiredService<DeleteNote>();

                await deleteNote.Delete(note.Id);

                RemoveNoteFromActiveCase(note.Id);

                await _state.NotesGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Notka: {note.Name} została usunięta.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        #endregion

        #region Cases Scheduler

        private async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createReminder = scope.ServiceProvider.GetRequiredService<CreateReminder>();

                ReminderViewModel result = await _dialogService.OpenAsync<AddReminderPage>("Dodaj Przypomnienie",
                    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } },
                    new DialogOptions() { Width = "450px", Height = "630px", Left = "calc(20%)", Top = "calc(25px)" });

                if (result != null)
                {
                    result.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

                    var newReminder = await createReminder.Create(_state.SelectedCase.Id, new CreateReminder.Request
                    {
                        IsDeadline = result.IsDeadline,
                        Message = result.Message,
                        Name = result.Name,
                        Priority = result.Priority == 0 ? 5 : result.Priority,
                        Start = result.Start,
                        End = result.End < result.Start ? result.Start : result.End,
                        UpdatedBy = result.UpdatedBy
                    });

                    AddReminderToActiveCase(newReminder);

                    await _state.CasesScheduler.Reload();
                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {newReminder.Name} zostało dodane.", GeneralViewModel.NotificationDuration);
                }
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteReminder = scope.ServiceProvider.GetRequiredService<DeleteReminder>();
                var updateReminder = scope.ServiceProvider.GetRequiredService<UpdateReminder>();

                ReminderViewModel result = await _dialogService.OpenAsync<EditReminderPage>($"Edytuj Przypomnienie dla Sprawy: {args.Data.ParentCaseName}",
                    new Dictionary<string, object> { { "Reminder", args.Data } },
                    new DialogOptions() { Width = "450px", Height = "630px", Left = "calc(20%)", Top = "calc(25px)" });

                if (result != null)
                {
                    if (!result.Active)
                    {
                        await deleteReminder.Delete(result.Id);

                        RemoveReminderFromActiveCase(result.Id);

                        await _state.CasesScheduler.Reload();
                        ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Przypomnienie: {result.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
                    }
                    else
                    {
                        var updatedReminder = await updateReminder.Update(new UpdateReminder.Request
                        {
                            Id = result.Id,
                            IsDeadline = result.IsDeadline,
                            Message = result.Message,
                            Name = result.Name,
                            Priority = result.Priority,
                            Start = result.Start,
                            End = result.End < result.Start ? result.Start : result.End,
                            Updated = DateTime.Now,
                            UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                        });

                        RemoveReminderFromActiveCase(updatedReminder.Id);
                        AddReminderToActiveCase(updatedReminder);

                        await _state.CasesScheduler.Reload();
                        ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {result.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
                    }
                }
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<ReminderViewModel> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.IsDeadline)
            {
                args.Attributes["style"] = $"background: {_generalViewModel.DeadlineColor};";
            }
            else
            {
                var scheme = _generalViewModel.PrioritiesColors.FirstOrDefault(x => x.Number == args.Data.Priority);
                args.Attributes["style"] = $"background: {scheme?.BackgroundColor}; color: {scheme?.TextColor};";
            }
        }

        #endregion

        #region Contact

        private void EditContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.EditRow(contact);

        private async Task OnUpdateContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateContactPerson = scope.ServiceProvider.GetRequiredService<UpdateContactPerson>();

                var result = await updateContactPerson.UpdateForCase(new UpdateContactPerson.Request
                {
                    Id = contact.Id,
                    Address = contact.Address,
                    Email = contact.Email,
                    Name = contact.Name,
                    Surname = contact.Surname,
                    PhoneNumber = contact.PhoneNumber,
                    AlternativePhoneNumber = contact.AlternativePhoneNumber,
                    Updated = DateTime.Now,
                    UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName
                });

                ReplaceContactFromActiveCase(result);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void SaveContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.UpdateRow(contact);

        private void CancelContactEdit(ContactPersonViewModel contact)
        {
            _state.ContactsGrid.CancelEditRow(contact);
            MainStore.RefreshMainComponent();
            BroadcastStateChange();
        }

        private async Task DeleteContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteContactPerson = scope.ServiceProvider.GetRequiredService<DeleteContactPerson>();

                await deleteContactPerson.DeleteForCase(contact.Id);
                RemoveContactFromActiveCase(contact.Id);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kontakt: {contact.Name} {contact.Surname} został usunięty.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createContactPerson = scope.ServiceProvider.GetRequiredService<CreateContactPerson>();

                var result = await createContactPerson.CreateContactPersonForCase(_state.SelectedCase.Id, request);
                _state.NewContact = new CreateContactPerson.Request();

                AddContactToActiveCase(result);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został dodany.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private void ContactSelected(object value)
        {
            var input = (ContactPersonViewModel)value;
            if (value != null)
            {
                _state.SelectedContact = _state.SelectedCase.ContactPeople.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedContact = null;
            }
        }

        #endregion

        public override void CleanUpStore()
        {
            _state.SelectedCase = null;
            _state.SelectedContact = null;
            _state.SelectedNote = null;
        }

        public override void RefreshSore()
        {
            GetCases(MainStore.GetState().ActiveClient.Id);
        }



    }
}
