using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Calendar;
using SWP.UI.BlazorApp.LegalApp.Stores.Calendar.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpCalendar : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public CalendarStore CalendarStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MainStore.RemoveStateChangeListener(RefreshCalendarData);
            CalendarStore.RemoveStateChangeListener(UpdateView);
            CalendarStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            MainStore.AddStateChangeListener(RefreshCalendarData);
            CalendarStore.AddStateChangeListener(UpdateView);
            CalendarStore.Initialize();
        }

        #region Actions


        private void OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args) => ActionDispatcher.Dispatch(new OnAppointmentSelectAction { Args = args });

        private void ActiveReminderChange(object args) => ActionDispatcher.Dispatch(new ActiveReminderChangeAction { Reminder = args });

        private void RefreshCalendarData() => ActionDispatcher.Dispatch(new RefreshCalendarDataAction());

        #endregion


    }
}
