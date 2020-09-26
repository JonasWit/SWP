using Org.BouncyCastle.Math.EC.Rfc7748;
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
using Microsoft.Extensions.DependencyInjection;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class CalendarPage : BlazorPageBase, IDisposable
    {
        private readonly DialogService dialogService;
        private readonly IServiceProvider serviceProvider;
        private readonly GeneralViewModel generalViewModel;

        private GetReminders GetReminders => serviceProvider.GetService<GetReminders>();
        private UpdateReminder UpdateReminder => serviceProvider.GetService<UpdateReminder>();
        private DeleteReminder DeleteReminder => serviceProvider.GetService<DeleteReminder>();
        private GetCase GetCase => serviceProvider.GetService<GetCase>();

        public LegalBlazorApp App { get; private set; }

        public CalendarPage(
            DialogService dialogService,
            IServiceProvider serviceProvider,
            GeneralViewModel generalViewModel)
        {
            this.dialogService = dialogService;
            this.serviceProvider = serviceProvider;
            this.generalViewModel = generalViewModel;
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
            App.ActiveClientChanged += new EventHandler(ActiveClientHasChanged);
        }

        private void UnsubscribeEvents()
        {
            App.ActiveClientChanged -= new EventHandler(ActiveClientHasChanged);
        }

        #region Reminders Calendar

        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }

        public List<ReminderViewModel> UpcomingReminders =>
            Reminders
            .Where(x => x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14))
            .ToList();

        public List<ReminderViewModel> UpcomingDeadlines =>
            Reminders
            .Where(x => x.IsDeadline && x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14))
            .ToList();

        public void RefreshCalendarData()
        {
            if (App.ActiveClient != null)
            {
                Reminders = GetReminders.Get(App.ActiveClient.Id).Select(x => (ReminderViewModel)x).ToList();
            }
            else
            {
                Reminders = GetReminders.Get(App.User.Profile).Select(x => (ReminderViewModel)x).ToList();
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            ReminderViewModel result = await dialogService.OpenAsync<EditReminderPage>($"Client: {GetCase.GetCaseParentName(args.Data.CaseId)} Case: {GetCase.GetCaseName(args.Data.CaseId)}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    await DeleteReminder.Delete(result.Id);
                    Reminders.RemoveAll(x => x.Id == result.Id);

                    if (App.ActiveClientWithData != null)
                    {
                        var c = App.ActiveClientWithData.Cases.FirstOrDefault(x => x.Reminders.Any(y => y.Id == result.Id));

                        if (c != null)
                        {
                            c.Reminders.RemoveAll(x => x.Id == result.Id);
                        }
                    }

                    await RemindersScheduler.Reload();
                    App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Przypomnienie: {result.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
                }
                else
                {
                    var updatedEntity = await UpdateReminder.Update(new UpdateReminder.Request
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

                    if (App.ActiveClientWithData != null)
                    {
                        var c = App.ActiveClientWithData.Cases.FirstOrDefault(x => x.Reminders.Any(y => y.Id == result.Id));

                        if (c != null)
                        {
                            c.Reminders.RemoveAll(x => x.Id == result.Id);
                            c.Reminders.Add(updatedEntity);
                        }
                    }

                    await RemindersScheduler.Reload();
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

        private void ActiveClientHasChanged(object sender, EventArgs e) => RefreshCalendarData();

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        #endregion
    }
}
