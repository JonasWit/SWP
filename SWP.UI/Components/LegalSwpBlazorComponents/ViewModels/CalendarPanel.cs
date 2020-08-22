using Radzen;
using Radzen.Blazor;
using SWP.Application;
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
        private readonly UpdateReminder updateReminder;
        private readonly DeleteReminder deleteReminder;
        private readonly GeneralViewModel generalViewModel;

        public LegalSwpApp App { get; private set; }

        public CalendarPanel(
            DialogService dialogService,
            GetReminders getReminders,
            UpdateReminder updateReminder,
            DeleteReminder deleteReminder,
            GeneralViewModel generalViewModel)
        {
            this.dialogService = dialogService;
            this.getReminders = getReminders;
            this.updateReminder = updateReminder;
            this.deleteReminder = deleteReminder;
            this.generalViewModel = generalViewModel;
        }

        public void Initialize(LegalSwpApp app) => App = app;

        #region Reminders Calendar

        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public List<ReminderViewModel> Reminders =>
            App.ActiveCustomerWithData != null ?
                App.ActiveCustomerWithData.Cases.SelectMany(x => x.Reminders).ToList() : getReminders.Get(App.User.Profile).Select(x => (ReminderViewModel)x).ToList();

        public async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<AddReminderPage>("Add Reminder",
                new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

            if (result != null)
            {


                // Either call the Reload method or reassign the Data property of the Scheduler
                await RemindersScheduler.Reload();
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
                    //reminders.RemoveAll(x => x.Id == result.Id);
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

                    //reminders = _getReminders.Get(User.Profile).Select(x => (ReminderViewModel)x).ToList();
                }

                // Either call the Reload method or reassign the Data property of the Scheduler
                await RemindersScheduler.Reload();
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

        #region Reminders Grid

        public RadzenGrid<ReminderViewModel> RemindersGrid { get; set; }

        public void EditReminderRow(ReminderViewModel reminder)
        {
        }

        public async Task OnUpdateReminderRow(ReminderViewModel reminder)
        {

        }

        public void SaveReminderRow(ReminderViewModel reminder)
        {

        }

        public void CancelReminderEdit(ReminderViewModel reminder)
        {

        }

        public async Task DeleteReminderRow(ReminderViewModel reminder)
        {

        }





        #endregion

    }
}
