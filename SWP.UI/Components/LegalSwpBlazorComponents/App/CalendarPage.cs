using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
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
    public class CalendarPage : BlazorPageBase, IDisposable
    {
        private readonly DialogService _dialogService;
        public GeneralViewModel _generalViewModel;
        public RadzenGrid<ReminderViewModel> RemindersGrid { get; set; }
        public LegalBlazorApp App { get; private set; }
        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public int ChosenReminderType { get; set; } = 3;
        public ReminderViewModel SelectedReminder { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }

        public CalendarPage(IServiceProvider serviceProvider, DialogService dialogService, GeneralViewModel generalViewModel) : base(serviceProvider) 
        {
            _dialogService = dialogService;
            _generalViewModel = generalViewModel;
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
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getReminders = scope.ServiceProvider.GetRequiredService<GetReminders>();

                if (App.ActiveClient != null)
                {
                    Reminders = getReminders.Get(App.ActiveClient.Id).Select(x => (ReminderViewModel)x).ToList();
                }
                else
                {
                    Reminders = getReminders.Get(App.User.Profile).Select(x => (ReminderViewModel)x).ToList();
                }

                UpdateRemindersData();
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        private void UpdateRemindersData()
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            foreach (var reminder in Reminders)
            {
                reminder.ParentCaseName = getCase.GetCaseName(reminder.CaseId);
                reminder.ParentClientName = getCase.GetCaseParentName(reminder.CaseId);
            }
        }

        public async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();


            ReminderViewModel result = await _dialogService.OpenAsync<EditReminderPage>($"Client: {getCase.GetCaseParentName(args.Data.CaseId)} Case: {getCase.GetCaseName(args.Data.CaseId)}", new Dictionary<string, object> { { "Reminder", args.Data } });

            if (result != null)
            {
                if (!result.Active)
                {
                    var deleteReminder = scope.ServiceProvider.GetRequiredService<DeleteReminder>();

                    await deleteReminder.Delete(result.Id);
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
                    var updateReminder = scope.ServiceProvider.GetRequiredService<UpdateReminder>();

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

                    Reminders[App.CalendarPage.Reminders.FindIndex(x => x.Id == result.Id)] = updatedReminder;

                    if (App.ActiveClientWithData != null)
                    {
                        var c = App.ActiveClientWithData.Cases.FirstOrDefault(x => x.Reminders.Any(y => y.Id == result.Id));

                        if (c != null)
                        {
                            App.ActiveClientWithData.SelectedCase.Reminders[App.ActiveClientWithData.SelectedCase.Reminders.FindIndex(x => x.Id == result.Id)] = updatedReminder;
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
            var commonStyle = $"";

            if (args.Data.IsDeadline)
            {
                args.Attributes["style"] = $"background: {_generalViewModel.DeadlineColor}; {commonStyle}";
            }
            else
            {
                var scheme = _generalViewModel.PrioritiesColors.FirstOrDefault(x => x.Number == args.Data.Priority);
                args.Attributes["style"] = $"background: {scheme?.BackgroundColor}; color: {scheme?.TextColor}; {commonStyle}";
            }
        }

        private void ActiveClientHasChanged(object sender, EventArgs e) => RefreshCalendarData();

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        public void ActiveReminderChange(object value)
        {
            var input = (ReminderViewModel)value;
            if (input != null)
            {
                SelectedReminder = Reminders.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                SelectedReminder = null;
            }
        }

        #endregion
    }
}
