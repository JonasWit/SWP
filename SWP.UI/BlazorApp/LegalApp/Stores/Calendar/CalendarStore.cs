using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Reminders;
using SWP.UI.BlazorApp.LegalApp.Stores.Calendar.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Calendar
{
    public class CalendarState
    {
        public RadzenGrid<ReminderViewModel> RemindersGrid { get; set; }
        public RadzenScheduler<ReminderViewModel> RemindersScheduler { get; set; }
        public int ChosenReminderType { get; set; } = 3;
        public ReminderViewModel SelectedReminder { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }
    }

    [UIScopedService]
    public class CalendarStore : StoreBase<CalendarState>
    {
        private readonly GeneralViewModel _generalViewModel;

        public MainStore MainStore => _serviceProvider.GetRequiredService<MainStore>();

        public CalendarStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService, GeneralViewModel generalViewModel) 
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {
            _generalViewModel = generalViewModel;
        }

        public void Initialize()
        {
            RefreshCalendarData();
        }

        public void ClearSelectedReminder() => _state.SelectedReminder = null;

        #region Reminders Calendar

        public List<ReminderViewModel> UpcomingReminders =>
            _state.Reminders.Where(x => x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14)).ToList();

        public List<ReminderViewModel> UpcomingDeadlines =>
            _state.Reminders.Where(x => x.IsDeadline && x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14)).ToList();

        private void RefreshCalendarData()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getReminders = scope.ServiceProvider.GetRequiredService<GetReminders>();

                if (MainStore.GetState().ActiveClient != null)
                {
                    _state.Reminders = getReminders.Get(MainStore.GetState().ActiveClient.Id).Select(x => (ReminderViewModel)x).ToList();
                }
                else
                {
                    _state.Reminders = getReminders.Get(MainStore.GetState().AppActiveUserManager.ProfileName).Select(x => (ReminderViewModel)x).ToList();
                }

                UpdateRemindersData();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        public void UpdateRemindersData()
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            foreach (var reminder in _state.Reminders)
            {
                if (string.IsNullOrEmpty(reminder.ParentCaseName))
                {
                    reminder.ParentCaseName = getCase.GetCaseName(reminder.CaseId);
                }

                if (string.IsNullOrEmpty(reminder.ParentClientName))
                {
                    reminder.ParentClientName = getCase.GetCaseParentName(reminder.CaseId);
                }
            }

            BroadcastStateChange();
        }

        private async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args)
        {
            using var scope = _serviceProvider.CreateScope();
            var getCase = scope.ServiceProvider.GetRequiredService<GetCase>();

            ReminderViewModel result = await _dialogService.OpenAsync<EditReminderPage>($"Client: " +
                $"{getCase.GetCaseParentName(args.Data.CaseId)} Case: {getCase.GetCaseName(args.Data.CaseId)}", new Dictionary<string, object> { { "Reminder", args.Data } }, 
                new DialogOptions() { Width = "450px", Height = "630px", Left = "calc(20%)", Top = "calc(25px)" });

            if (result != null)
            {
                if (!result.Active)
                {
                    var deleteReminder = scope.ServiceProvider.GetRequiredService<DeleteReminder>();

                    await deleteReminder.Delete(result.Id);
                    _state.Reminders.RemoveAll(x => x.Id == result.Id);

                    await _state.RemindersScheduler.Reload();
                    ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Przypomnienie: {result.Name} zostało usunięte.", GeneralViewModel.NotificationDuration);
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
                        UpdatedBy = MainStore.GetApplicationUser().User.UserName,
                    });

                    _state.Reminders[_state.Reminders.FindIndex(x => x.Id == result.Id)] = updatedReminder;

                    UpdateRemindersData();
                    await _state.RemindersScheduler.Reload();
                    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Przypomnienie: {result.Name} zostało zmienione.", GeneralViewModel.NotificationDuration);
                }
            }

            BroadcastStateChange();
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

        private void ActiveReminderChange(object value)
        {
            var input = (ReminderViewModel)value;
            if (input != null)
            {
                _state.SelectedReminder = _state.Reminders.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                _state.SelectedReminder = null;
            }
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case OnAppointmentSelectAction.OnAppointmentSelect:
                    var actionTypeI = (OnAppointmentSelectAction)action;
                    await OnAppointmentSelect(actionTypeI.Args);
                    break;
                case ActiveReminderChangeAction.ActiveReminderChange:
                    var actionTypeII = (ActiveReminderChangeAction)action;
                    ActiveReminderChange(actionTypeII.Reminder);
                    break;
                case RefreshCalendarDataAction.RefreshCalendarData:
                    RefreshCalendarData();
                    break;
                default:
                    break;
            }
        }

        public void CleanUpStore()
        {
            _state.SelectedReminder = null;
        }

        public void RefreshSore()
        {
            _state.SelectedReminder = null;
            RefreshCalendarData();
        }

        #endregion
    }
}
