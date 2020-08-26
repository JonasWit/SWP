using Radzen;
using Radzen.Blazor;
using SWP.Application;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class CalendarPanel
    {
        private readonly DialogService dialogService;
        private readonly GetReminders getReminders;
        private readonly GetReminder getReminder;
        private readonly UpdateReminder updateReminder;
        private readonly DeleteReminder deleteReminder;
        private readonly GeneralViewModel generalViewModel;
        private readonly GetCase getCase;

        public LegalSwpApp App { get; private set; }

        public CalendarPanel(
            DialogService dialogService,
            GetReminders getReminders,
            GetReminder getReminder,
            UpdateReminder updateReminder,
            DeleteReminder deleteReminder,
            GeneralViewModel generalViewModel,
            GetCase getCase)
        {
            this.dialogService = dialogService;
            this.getReminders = getReminders;
            this.getReminder = getReminder;
            this.updateReminder = updateReminder;
            this.deleteReminder = deleteReminder;
            this.generalViewModel = generalViewModel;
            this.getCase = getCase;
        }

        public void Initialize(LegalSwpApp app)
        { 
            App = app;
            RefreshCalendarData();
        } 

        #region Reminders Calendar

        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }

        public void RefreshCalendarData() => Reminders = getReminders.Get(App.User.Profile).Select(x => (ReminderViewModel)x).ToList();        

        public async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            //ReminderViewModel result = await dialogService.OpenAsync<AddReminderPage>("Add Reminder",
            //    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

            //if (result != null)
            //{
            //    // Either call the Reload method or reassign the Data property of the Scheduler
            //    await RemindersScheduler.Reload();
            //}
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<EditReminderPage>($"Edit Reminder for Case: {args.Data.ParentCaseName}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    await deleteReminder.Delete(result.Id);
                    RefreshCalendarData();
                }
                else
                {
                    await updateReminder.Update(new UpdateReminder.Request
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

                    RefreshCalendarData();
                }

                if (App.ActiveCustomerWithData != null)
                {
                    ReloadCase(result.CaseId);
                }

                // Either call the Reload method or reassign the Data property of the Scheduler
                await RemindersScheduler.Reload();
            }
        }

        public void ReloadCase(int id)
        {
            var caseEntity = getCase.Get(id);
            App.ActiveCustomerWithData.Cases.RemoveAll(x => x.Id == id);
            App.ActiveCustomerWithData.Cases.Add(caseEntity);
            App.ActiveCustomerWithData.Cases = App.ActiveCustomerWithData.Cases.OrderBy(x => x.Name).ToList();
            App.ActiveCustomerWithData.Cases.TrimExcess();
            App.ActiveCustomerWithData.SelectedCase = caseEntity;
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
