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

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CasesPage : BlazorPageBase
    {
        private readonly DialogService dialogService;
        private readonly GeneralViewModel generalViewModel;

        private CreateReminder CreateReminder => serviceProvider.GetService<CreateReminder>();
        private UpdateReminder UpdateReminder => serviceProvider.GetService<UpdateReminder>();
        private DeleteReminder DeleteReminder => serviceProvider.GetService<DeleteReminder>();
        private GetCase GetCase => serviceProvider.GetService<GetCase>();
        private CreateCase CreateCase => serviceProvider.GetService<CreateCase>();
        private UpdateCase UpdateCase => serviceProvider.GetService<UpdateCase>();
        private DeleteCase DeleteCase => serviceProvider.GetService<DeleteCase>();
        private CreateNote CreateNote => serviceProvider.GetService<CreateNote>();
        private DeleteNote DeleteNote => serviceProvider.GetService<DeleteNote>();
        private UpdateNote UpdateNote => serviceProvider.GetService<UpdateNote>();

        public LegalBlazorApp App { get; private set; }

        public CasesPage(
                DialogService dialogService,
                GeneralViewModel generalViewModel,
                IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.dialogService = dialogService;
            this.generalViewModel = generalViewModel;
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

        #region Cases Management

        public void ReloadCase(int id)
        {
            var caseEntity = GetCase.Get(id);
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
                var result = await CreateCase.Create(App.ActiveClientWithData.Id, App.User.Profile, NewCase);
                NewCase = new CreateCase.Request();

                App.ActiveClientWithData.Cases.Add(result);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void EditCaseRow(CaseViewModel c) => CasesGrid.EditRow(c);

        public async Task OnUpdateCaseRow(CaseViewModel c)
        {
            try
            {
                var result = await UpdateCase.Update(new UpdateCase.Request
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
                App.ErrorPage.DisplayMessage(ex);
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
                await DeleteCase.Delete(c.Id);

                App.ActiveClientWithData.Cases.RemoveAll(x => x.Id == c.Id);
                await CasesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Sprawa: {c.Name} została usunięta.", GeneralViewModel.NotificationDuration);

            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
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
                var result = await CreateNote.Create(App.ActiveClientWithData.SelectedCase.Id, NewNote);
                NewNote = new CreateNote.Request();

                App.ActiveClientWithData.SelectedCase.Notes.Add(result);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Notka: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void EditNoteRow(NoteViewModel note) => NotesGrid.EditRow(note);

        public async Task OnUpdateNoteRow(NoteViewModel note)
        {
            try
            {
                var result = await UpdateNote.Update(new UpdateNote.Request
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
                App.ErrorPage.DisplayMessage(ex);
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
                await DeleteNote.Delete(note.Id);
                App.ActiveClientWithData.SelectedCase.Notes.RemoveAll(x => x.Id == note.Id);
                await NotesGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Notka: {note.Name} została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
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

                var newReminder = await CreateReminder.Create(App.ActiveClientWithData.SelectedCase.Id, new CreateReminder.Request
                {
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
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {newReminder.Name} zostało dodane.", GeneralViewModel.NotificationDuration);
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<EditReminderPage>($"Edit Reminder for Case: {args.Data.ParentCaseName}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    await DeleteReminder.Delete(result.Id);
                    App.ActiveClientWithData.SelectedCase.Reminders.RemoveAll(x => x.Id == result.Id);
                    App.CalendarPage.Reminders.RemoveAll(x => x.Id == result.Id);

                    await CasesScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Przypomnienie: {result.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
                }
                else
                {
                    var updatedReminder = await UpdateReminder.Update(new UpdateReminder.Request
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
                    App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {result.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
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
