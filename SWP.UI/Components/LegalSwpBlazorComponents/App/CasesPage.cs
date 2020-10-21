using Microsoft.AspNetCore.Mvc;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Notes;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWP.Application.LegalSwp.ContactPeopleAdmin;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CasesPage : BlazorPageBase
    {
        private readonly GeneralViewModel _generalViewModel;
        private readonly DialogService _dialogService;
        public LegalBlazorApp App { get; private set; }
        public CreateCase.Request NewCase { get; set; } = new CreateCase.Request();
        public CreateNote.Request NewNote { get; set; } = new CreateNote.Request();
        public CreateContactPerson.Request NewContact { get; set; } = new CreateContactPerson.Request();
        public RadzenGrid<CaseViewModel> CasesGrid { get; set; }
        public RadzenGrid<NoteViewModel> NotesGrid { get; set; }
        public RadzenGrid<ContactPersonViewModel> ContactsGrid { get; set; }
        public RadzenScheduler<ReminderViewModel> CasesScheduler { get; set; }
        public ContactPersonViewModel SelectedContact { get; set; }

        public CasesPage(IServiceProvider serviceProvider, GeneralViewModel generalViewModel, DialogService dialogService) : base(serviceProvider) 
        {
            _generalViewModel = generalViewModel;
            _dialogService = dialogService;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        #region Cases Management

        public void ReloadCase(int id)
        {
            using var scope = serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            var caseEntity = getCase.Get(id);
            App.ActiveClientWithData.Cases.RemoveAll(x => x.Id == id);
            App.ActiveClientWithData.Cases.Add(caseEntity);
            App.ActiveClientWithData.Cases = App.ActiveClientWithData.Cases.OrderBy(x => x.Name).ToList();
            App.ActiveClientWithData.Cases.TrimExcess();
            App.ActiveClientWithData.SelectedCase = caseEntity;
        }

        public async Task CreateNewCase(CreateCase.Request request)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var createCase = scope.ServiceProvider.GetRequiredService<CreateCase>();

                request.UpdatedBy = App.User.UserName;
                var result = await createCase.Create(App.ActiveClientWithData.Id, App.User.Profile, request);
                NewCase = new CreateCase.Request();

                App.ActiveClientWithData.Cases.Add(result);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void EditCaseRow(CaseViewModel c) => CasesGrid.EditRow(c);

        public async Task OnUpdateCaseRow(CaseViewModel c)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var updateCase = scope.ServiceProvider.GetRequiredService<UpdateCase>();

                var result = await updateCase.Update(new UpdateCase.Request
                {
                    Id = c.Id,
                    CaseType = c.CaseType,
                    Description = c.Description,
                    Name = c.Name,
                    Signature = c.Signature,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.Cases[App.ActiveClientWithData.Cases.FindIndex(x => x.Id == result.Id)] = result;

                if (App.ActiveClientWithData.SelectedCase.Id == result.Id)
                {
                    App.ActiveClientWithData.SelectedCase = App.ActiveClientWithData.Cases.FirstOrDefault(x => x.Id == result.Id);
                }

                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveCaseRow(CaseViewModel c) => CasesGrid.UpdateRow(c);

        public void CancelEditCaseRow(CaseViewModel c)
        {
            CasesGrid.CancelEditRow(c);
            App.RefreshClientWithData();
        }

        public async Task DeleteCaseRow(CaseViewModel c)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var deleteCase = scope.ServiceProvider.GetRequiredService<DeleteCase>();

                await deleteCase.Delete(c.Id);

                App.ActiveClientWithData.Cases.RemoveAll(x => x.Id == c.Id);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Sprawa: {c.Name} została usunięta.", GeneralViewModel.NotificationDuration);

            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveCaseChange(object value)
        {
            var input = (CaseViewModel)value;
            if (value != null)
            {
                App.ActiveClientWithData.SelectedCase = App.ActiveClientWithData.Cases.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedCase = null;
            }
        }

        #endregion

        #region Notes Tab

        public void ActiveNoteChange(object value)
        {
            var input = (NoteViewModel)value;
            if (value != null)
            {
                App.ActiveClientWithData.SelectedCase.SelectedNote = App.ActiveClientWithData.SelectedCase.Notes.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedCase.SelectedNote = null;
            }
        }

        public async Task CreateNewNote(CreateNote.Request request)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var createNote = scope.ServiceProvider.GetRequiredService<CreateNote>();

                request.UpdatedBy = App.User.UserName;
                var result = await createNote.Create(App.ActiveClientWithData.SelectedCase.Id, request);
                NewNote = new CreateNote.Request();

                App.ActiveClientWithData.SelectedCase.Notes.Add(result);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void EditNoteRow(NoteViewModel note) => NotesGrid.EditRow(note);

        public async Task OnUpdateNoteRow(NoteViewModel note)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var updateNote = scope.ServiceProvider.GetRequiredService<UpdateNote>();

                var result = await updateNote.Update(new UpdateNote.Request
                {
                    Id = note.Id,
                    Message = note.Message,
                    Name = note.Name,
                    Priority = note.Priority,
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.SelectedCase.Notes[App.ActiveClientWithData.SelectedCase.Notes.FindIndex(x => x.Id == result.Id)] = result;
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveNoteRow(NoteViewModel note) => NotesGrid.UpdateRow(note);

        public void CancelEditNoteRow(NoteViewModel note)
        {
            NotesGrid.CancelEditRow(note);
            ReloadCase(note.CaseId);
        }

        public async Task DeleteNoteRow(NoteViewModel note)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var deleteNote = scope.ServiceProvider.GetRequiredService<DeleteNote>();

                await deleteNote.Delete(note.Id);
                App.ActiveClientWithData.SelectedCase.Notes.RemoveAll(x => x.Id == note.Id);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Notka: {note.Name} została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        #endregion

        #region Cases Scheduler

        public async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var createReminder = scope.ServiceProvider.GetRequiredService<CreateReminder>();

                ReminderViewModel result = await _dialogService.OpenAsync<AddReminderPage>("Dodaj Przypomnienie",
                    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } },
                    new DialogOptions() { Width = "500px", Height = "530px", Left = "calc(50% - 500px)", Top = "calc(50% - 265px)" });

                if (result != null)
                {
                    result.UpdatedBy = App.User.UserName;

                    var newReminder = await createReminder.Create(App.ActiveClientWithData.SelectedCase.Id, new CreateReminder.Request
                    {
                        IsDeadline = result.IsDeadline,
                        Message = result.Message,
                        Name = result.Name,
                        Priority = result.Priority == 0 ? 5 : result.Priority,
                        Start = result.Start,
                        End = result.End < result.Start ? result.Start : result.End,
                        UpdatedBy = result.UpdatedBy
                    });

                    App.ActiveClientWithData.SelectedCase.Reminders.Add(newReminder);
                    App.CalendarPage.Reminders.Add(newReminder);
                    await CasesScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {newReminder.Name} zostało dodane.", GeneralViewModel.NotificationDuration);
                }
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
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
                        App.ActiveClientWithData.SelectedCase.Reminders.RemoveAll(x => x.Id == result.Id);
                        App.CalendarPage.Reminders.RemoveAll(x => x.Id == result.Id);

                        await CasesScheduler.Reload();
                        App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Przypomnienie: {result.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
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
                            UpdatedBy = App.User.UserName
                        });

                        App.ActiveClientWithData.SelectedCase.Reminders[App.ActiveClientWithData.SelectedCase.Reminders.FindIndex(x => x.Id == result.Id)] = updatedReminder;
                        App.CalendarPage.Reminders[App.CalendarPage.Reminders.FindIndex(x => x.Id == result.Id)] = updatedReminder;

                        await CasesScheduler.Reload();
                        App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {result.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
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

        public void EditContactRow(ContactPersonViewModel contact) => ContactsGrid.EditRow(contact);

        public async Task OnUpdateContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
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
                    UpdatedBy = App.User.UserName
                });

                App.ActiveClientWithData.SelectedCase.ContactPeople[App.ActiveClientWithData.SelectedCase.ContactPeople.FindIndex(x => x.Id == result.Id)] = result;

                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveContactRow(ContactPersonViewModel contact) => ContactsGrid.UpdateRow(contact);

        public void CancelContactEdit(ContactPersonViewModel contact)
        {
            ContactsGrid.CancelEditRow(contact);
            App.RefreshClientWithData();
        }

        public async Task DeleteContactRow(ContactPersonViewModel contact)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var deleteContactPerson = scope.ServiceProvider.GetRequiredService<DeleteContactPerson>();

                await deleteContactPerson.DeleteForCase(contact.Id);
                App.ActiveClientWithData.SelectedCase.ContactPeople.RemoveAll(x => x.Id == contact.Id);

                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kontakt: {contact.Name} {contact.Surname} został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewContact(CreateContactPerson.Request request)
        {
            request.UpdatedBy = App.User.UserName;

            try
            {
                using var scope = serviceProvider.CreateScope();
                var createContactPerson = scope.ServiceProvider.GetRequiredService<CreateContactPerson>();

                var result = await createContactPerson.CreateContactPersonForCase(App.ActiveClientWithData.SelectedCase.Id, request);
                NewContact = new CreateContactPerson.Request();

                App.ActiveClientWithData.SelectedCase.ContactPeople.Add(result);
                await ContactsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kontakt: {result.Name} {result.Surname} został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ContactSelected(object value)
        {
            var input = (ContactPersonViewModel)value;
            if (value != null)
            {
                SelectedContact = App.ActiveClientWithData.SelectedCase.ContactPeople.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                SelectedContact = null;
            }
        }

        #endregion
    }
}
