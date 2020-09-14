﻿using Org.BouncyCastle.Math.EC.Rfc7748;
using Radzen;
using Radzen.Blazor;
using SWP.Application;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CalendarPage : BlazorPageBase, IDisposable
    {
        private readonly DialogService dialogService;
        private readonly GetReminders getReminders;
        private readonly UpdateReminder updateReminder;
        private readonly DeleteReminder deleteReminder;
        private readonly GeneralViewModel generalViewModel;
        private readonly GetCase getCase;

        public LegalBlazorApp App { get; private set; }

        public CalendarPage(
            DialogService dialogService,

            GetReminders getReminders,
            UpdateReminder updateReminder,
            DeleteReminder deleteReminder,
            GeneralViewModel generalViewModel,
            GetCase getCase)
        {
            this.dialogService = dialogService;
            this.getReminders = getReminders;
            this.updateReminder = updateReminder;
            this.deleteReminder = deleteReminder;
            this.generalViewModel = generalViewModel;
            this.getCase = getCase;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            RefreshCalendarData();
            SubscribeToEvents();
            return Task.CompletedTask;
        }

        private void SubscribeToEvents()
        {
            App.ActiveCustomerChanged += new EventHandler(ActiveCustomerHasChanged);
        }

        private void UnsubscribeEvents()
        {
            App.ActiveCustomerChanged -= new EventHandler(ActiveCustomerHasChanged);
        }

        #region Reminders Calendar

        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }

        public void RefreshCalendarData()
        {
            if (App.ActiveCustomer != null)
            {
                Reminders = getReminders.Get(App.ActiveCustomer.Id).Select(x => (ReminderViewModel)x).ToList();
            }
            else
            {
                Reminders = getReminders.Get(App.User.Profile).Select(x => (ReminderViewModel)x).ToList();
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<EditReminderPage>($"Customer: {getCase.GetCaseParentName(args.Data.CaseId)} Case: {getCase.GetCaseName(args.Data.CaseId)}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    await deleteReminder.Delete(result.Id);
                    Reminders.RemoveAll(x => x.Id == result.Id);

                    if (App.ActiveCustomerWithData != null)
                    {
                        var c = App.ActiveCustomerWithData.Cases.FirstOrDefault(x => x.Reminders.Any(y => y.Id == result.Id));

                        if (c != null)
                        {
                            c.Reminders.RemoveAll(x => x.Id == result.Id);
                        }
                    }

                    await RemindersScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Warning, "Success!", $"Reminder: {result.Name} has been deleted.", 2000);
                }
                else
                {
                    var updatedEntity = await updateReminder.Update(new UpdateReminder.Request
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

                    Reminders.RemoveAll(x => x.Id == updatedEntity.Id);
                    Reminders.Add(updatedEntity);

                    if (App.ActiveCustomerWithData != null)
                    {
                        var c = App.ActiveCustomerWithData.Cases.FirstOrDefault(x => x.Reminders.Any(y => y.Id == result.Id));

                        if (c != null)
                        {
                            c.Reminders.RemoveAll(x => x.Id == result.Id);
                            c.Reminders.Add(updatedEntity);
                        }
                    }

                    await RemindersScheduler.Reload();
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

        private void ActiveCustomerHasChanged(object sender, EventArgs e) => RefreshCalendarData();

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        #endregion
    }
}