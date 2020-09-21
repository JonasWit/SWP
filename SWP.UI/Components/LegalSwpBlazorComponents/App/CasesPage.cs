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

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CasesPage : BlazorPageBase
    {
        public enum Panels
        {
            Admin = 0,
            AddCase = 1,
        }

        private readonly DialogService dialogService;
        private readonly CreateReminder createReminder;
        private readonly UpdateReminder updateReminder;
        private readonly DeleteReminder deleteReminder;
        private readonly GeneralViewModel generalViewModel;
        private readonly GetCase getCase;
        private readonly CreateCase createCase;
        private readonly UpdateCase updateCase;
        private readonly DeleteCase deleteCase;
        private readonly CreateNote createNote;
        private readonly DeleteNote deleteNote;
        private readonly UpdateNote updateNote;

        public LegalBlazorApp App { get; private set; }

        public CasesPage(
            DialogService dialogService,
            CreateReminder createReminder,
            UpdateReminder updateReminder,
            DeleteReminder deleteReminder,
            GeneralViewModel generalViewModel,
            GetCase getCase,
            CreateCase createCase,
            UpdateCase updateCase,
            DeleteCase deleteCase,
            CreateNote createNote,
            DeleteNote deleteNote,
            UpdateNote updateNote)
        {
            this.dialogService = dialogService;
            this.createReminder = createReminder;
            this.updateReminder = updateReminder;
            this.deleteReminder = deleteReminder;
            this.generalViewModel = generalViewModel;
            this.getCase = getCase;
            this.createCase = createCase;
            this.updateCase = updateCase;
            this.deleteCase = deleteCase;
            this.createNote = createNote;
            this.deleteNote = deleteNote;
            this.updateNote = updateNote;
            SetActivePanel(Panels.Admin);
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public CreateCase.Request NewCase { get; set; } = new CreateCase.Request();
        public CreateNote.Request NewNote { get; set; } = new CreateNote.Request();
        public RadzenGrid<CaseViewModel> CasesGrid { get; set; }
        public RadzenGrid<NoteViewModel> NotesGrid { get; set; }
        public RadzenScheduler<ReminderViewModel> CasesScheduler { get; set; }

        public Panels ActivePanel { get; set; }
        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        #region Cases Management

        public void ReloadCase(int id)
        {
            var caseEntity = getCase.Get(id);
            App.ActiveClientWithData.Cases.RemoveAll(x => x.Id == id);
            App.ActiveClientWithData.Cases.Add(caseEntity);
            App.ActiveClientWithData.Cases = App.ActiveClientWithData.Cases.OrderBy(x => x.Name).ToList();
            App.ActiveClientWithData.Cases.TrimExcess();
            App.ActiveClientWithData.SelectedCase = caseEntity;
        }

        public async Task CreateNewCase(CreateCase.Request arg)
        {
            try
            {
                NewCase.UpdatedBy = App.User.UserName;
                var result = await createCase.Create(App.ActiveClientWithData.Id, App.User.Profile, NewCase);
                NewCase = new CreateCase.Request();

                App.ActiveClientWithData.Cases.Add(result);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Case: {result.Name} has been added.", 2000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EditCaseRow(CaseViewModel c) => CasesGrid.EditRow(c);

        public async Task OnUpdateCaseRow(CaseViewModel c)
        {
            try
            {
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
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Case: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveCaseRow(CaseViewModel c) => CasesGrid.UpdateRow(c);

        public void CancelEditCaseRow(CaseViewModel c) => CasesGrid.CancelEditRow(c);

        public async Task DeleteCaseRow(CaseViewModel c)
        {
            try
            {
                await deleteCase.Delete(c.Id);

                App.ActiveClientWithData.Cases.RemoveAll(x => x.Id == c.Id);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Case: {c.Name} has been deleted.", 2000);

            }
            catch (Exception ex)
            {
                throw;
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

        public async Task CreateNewNote(CreateNote.Request arg)
        {
            try
            {
                NewNote.UpdatedBy = App.User.UserName;
                var result = await createNote.Create(App.ActiveClientWithData.SelectedCase.Id, NewNote);
                NewNote = new CreateNote.Request();

                App.ActiveClientWithData.SelectedCase.Notes.Add(result);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Note: {result.Name} has been added.", 2000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EditNoteRow(NoteViewModel note) => NotesGrid.EditRow(note);

        public async Task OnUpdateNoteRow(NoteViewModel note)
        {
            try
            {
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
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Note: {result.Name} has been updated.", 2000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveNoteRow(NoteViewModel note) => NotesGrid.UpdateRow(note);

        public void CancelEditNoteRow(NoteViewModel note) => NotesGrid.CancelEditRow(note);

        public async Task DeleteNoteRow(NoteViewModel note)
        {
            try
            {
                await deleteNote.Delete(note.Id);
                App.ActiveClientWithData.SelectedCase.Notes.RemoveAll(x => x.Id == note.Id);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Note: {note.Name} has been deleted.", 2000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Cases Scheduler

        public async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<AddReminderPage>("Add Reminder",
                new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

            if (result != null)
            {
                result.UpdatedBy = App.User.UserName;

                var newReminder = await createReminder.Create(App.ActiveClientWithData.SelectedCase.Id, new CreateReminder.Request
                {
                    Active = true,
                    IsDeadline = result.IsDeadline,
                    Message = result.Message,
                    Name = result.Name,
                    Priority = result.Priority == 0 ? 5 : result.Priority,
                    Start = result.Start,
                    End = result.End,
                    UpdatedBy = result.UpdatedBy
                });

                App.ActiveClientWithData.SelectedCase.Reminders.Add(newReminder);
                App.CalendarPage.Reminders.Add(newReminder);
                await CasesScheduler.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Success!", $"Reminder: {newReminder.Name} has been added.", 2000);
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<EditReminderPage>($"Edit Reminder for Case: {args.Data.ParentCaseName}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    await deleteReminder.Delete(result.Id);
                    App.ActiveClientWithData.SelectedCase.Reminders.RemoveAll(x => x.Id == result.Id);
                    App.CalendarPage.Reminders.RemoveAll(x => x.Id == result.Id);

                    await CasesScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Reminder: {result.Name} has been deleted.", 2000);
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
                        End = result.End,
                        Updated = DateTime.Now,
                        UpdatedBy = App.User.UserName
                    });

                    App.ActiveClientWithData.SelectedCase.Reminders[App.ActiveClientWithData.SelectedCase.Reminders.FindIndex(x => x.Id == result.Id)] = result;
                    App.CalendarPage.Reminders[App.CalendarPage.Reminders.FindIndex(x => x.Id == result.Id)] = result;

                    await CasesScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Success, "Success!", $"Reminder: {result.Name} has been updated.", 2000);
                }
            }
        }

        public void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<ReminderViewModel> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.IsDeadline)
            {
                args.Attributes["style"] = $"background: {generalViewModel.DeadlineColor};";
            }
            else
            {
                var scheme = generalViewModel.PrioritiesColors.FirstOrDefault(x => x.Number == args.Data.Priority);
                args.Attributes["style"] = $"background: {scheme?.BackgroundColor}; color: {scheme?.TextColor};";
            }
        }

        #endregion











    }
}
