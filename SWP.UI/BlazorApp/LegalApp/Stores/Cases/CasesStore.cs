using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.Application.LegalSwp.Notes;
using SWP.Application.LegalSwp.Reminders;
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

        private MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public CasesStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService, GeneralViewModel generalViewModel)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _generalViewModel = generalViewModel;
        }

        public void Initialize()
        {
            GetCases(_mainStore.GetState().ActiveClient.Id);
        }

        public void GetCases(int clientId)
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
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
        }

        public void ClearSelectedCase() => _state.SelectedCase = null;

        public void SetSelectedCase(CaseViewModel entity) => _state.SelectedCase = entity;

        public void SetSelectedNote(NoteViewModel entity) => _state.SelectedNote = entity;

        public void AddNoteToActiveCase(NoteViewModel entity) => _state.SelectedCase.Notes.Add(entity);

        public void RemoveNoteFromActiveCase(int id) => _state.SelectedCase.Notes.RemoveAll(x => x.Id == id);

        public void ReplaceNoteFromActiveCase(NoteViewModel entity) => _state.SelectedCase.Notes[_state.SelectedCase.Notes.FindIndex(x => x.Id == entity.Id)] = entity;

        public void AddReminderToActiveCase(ReminderViewModel entity) => _state.SelectedCase.Reminders.Add(entity);

        public void RemoveReminderFromActiveCase(int id) => _state.SelectedCase.Reminders.RemoveAll(x => x.Id == id);

        public void ReplaceReminderFromActiveCase(ReminderViewModel entity) => _state.SelectedCase.Reminders[_state.SelectedCase.Reminders.FindIndex(x => x.Id == entity.Id)] = entity;

        public void AddContactToActiveCase(ContactPersonViewModel entity) => _state.SelectedCase.ContactPeople.Add(entity);

        public void RemoveContactFromActiveCase(int id) => _state.SelectedCase.ContactPeople.RemoveAll(x => x.Id == id);

        public void ReplaceContactFromActiveCase(ContactPersonViewModel entity) => _state.SelectedCase.ContactPeople[_state.SelectedCase.ContactPeople.FindIndex(x => x.Id == entity.Id)] = entity;

        #region Cases Management

        public async Task CreateNewCase(CreateCase.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createCase = scope.ServiceProvider.GetRequiredService<CreateCase>();

                request.UpdatedBy = _mainStore.GetState().User.UserName;
                var result = await createCase.Create(_mainStore.GetState().ActiveClient.Id, _mainStore.GetState().User.Profile, request);
                _state.NewCase = new CreateCase.Request();

                AddCaseToActiveClient(result);

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void EditCaseRow(CaseViewModel c) => _state.CasesGrid.EditRow(c);

        public async Task OnUpdateCaseRow(CaseViewModel c)
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
                    UpdatedBy = _mainStore.GetState().User.UserName
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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SaveCaseRow(CaseViewModel c) => _state.CasesGrid.UpdateRow(c);

        public void CancelEditCaseRow(CaseViewModel c)
        {
            _state.CasesGrid.CancelEditRow(c);
            _mainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteCaseRow(CaseViewModel c)
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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void ActiveCaseChange(object value)
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

        public async Task ArchivizeCase(CaseViewModel c)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var archiveCase = scope.ServiceProvider.GetRequiredService<ArchiveCases>();

                _state.Cases.RemoveAll(x => x.Id == c.Id);
                await archiveCase.ArchivizeCase(c.Id, _mainStore.GetState().User.UserName);

                await _state.CasesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {c.Name} została zarchwizowana", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void ReloadCase(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            var caseEntity = getCase.Get(id);
            _state.Cases.RemoveAll(x => x.Id == id);
            _state.Cases.Add(caseEntity);
            _state.Cases = _state.Cases.OrderBy(x => x.Name).ToList();
            _state.Cases.TrimExcess();
        }

        public void AddCaseToActiveClient(CaseViewModel entity) => _state.Cases.Add(entity);

        public void RemoveCaseFromActiveClient(int id) => _state.Cases.RemoveAll(x => x.Id == id);

        public void ReplaceCaseFromActiveClient(CaseViewModel entity) => _state.Cases[_state.Cases.FindIndex(x => x.Id == entity.Id)] = entity;

        #endregion

        #region Notes Tab

        public void ActiveNoteChange(object value)
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

        public async Task CreateNewNote(CreateNote.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createNote = scope.ServiceProvider.GetRequiredService<CreateNote>();

                request.UpdatedBy = _mainStore.GetState().User.UserName;
                var result = await createNote.Create(_state.SelectedCase.Id, request);

                _state.NewNote = new CreateNote.Request();

                AddNoteToActiveCase(result);

                await _state.NotesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void EditNoteRow(NoteViewModel note) => _state.NotesGrid.EditRow(note);

        public async Task OnUpdateNoteRow(NoteViewModel note)
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
                    UpdatedBy = _mainStore.GetState().User.UserName
                });

                ReplaceNoteFromActiveCase(result);

                await _state.NotesGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SaveNoteRow(NoteViewModel note) => _state.NotesGrid.UpdateRow(note);

        public void CancelEditNoteRow(NoteViewModel note)
        {
            _state.NotesGrid.CancelEditRow(note);
            ReloadCase(note.CaseId);
            BroadcastStateChange();
        }

        public async Task DeleteNoteRow(NoteViewModel note)
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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        #endregion

        #region Cases Scheduler

        public async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createReminder = scope.ServiceProvider.GetRequiredService<CreateReminder>();

                ReminderViewModel result = await _dialogService.OpenAsync<AddReminderPage>("Dodaj Przypomnienie",
                    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } },
                    new DialogOptions() { Width = "500px", Height = "530px", Left = "calc(50% - 500px)", Top = "calc(50% - 265px)" });

                if (result != null)
                {
                    result.UpdatedBy = _mainStore.GetState().User.UserName;

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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteReminder = scope.ServiceProvider.GetRequiredService<DeleteReminder>();
                var updateReminder = scope.ServiceProvider.GetRequiredService<UpdateReminder>();

                ReminderViewModel result = await _dialogService.OpenAsync<EditReminderPage>($"Edytuj Przypomnienie dla Sprawy: {args.Data.ParentCaseName}",
                    new Dictionary<string, object> { { "Reminder", args.Data } },
                    new DialogOptions() { Width = "500px", Height = "630px", Left = "calc(50% - 500px)", Top = "calc(50% - 265px)" });

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
                            UpdatedBy = _mainStore.GetState().User.UserName
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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<ReminderViewModel> args)
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

        public void EditContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.EditRow(contact);

        public async Task OnUpdateContactRow(ContactPersonViewModel contact)
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
                    UpdatedBy = _mainStore.GetState().User.UserName
                });

                ReplaceContactFromActiveCase(result);

                await _state.ContactsGrid.Reload();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SaveContactRow(ContactPersonViewModel contact) => _state.ContactsGrid.UpdateRow(contact);

        public void CancelContactEdit(ContactPersonViewModel contact)
        {
            _state.ContactsGrid.CancelEditRow(contact);
            _mainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteContactRow(ContactPersonViewModel contact)
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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = _mainStore.GetState().User.UserName;

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
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void ContactSelected(object value)
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

        protected override void HandleActions(IAction action)
        {

        }

        public override void CleanUpStore()
        {
            _state.SelectedCase = null;
            _state.SelectedContact = null;
            _state.SelectedNote = null;
        }

        public override void RefreshSore()
        {
            GetCases(_mainStore.GetState().ActiveClient.Id);
        }

        #endregion
















    }
}
