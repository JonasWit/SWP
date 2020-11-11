using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Calendar;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpCalendar : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public CalendarStore CalendarStore { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MainStore.RemoveStateChangeListener(RefreshCalendar);
            CalendarStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();
 
        private void RefreshCalendar() => CalendarStore.RefreshCalendarData();

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            MainStore.AddStateChangeListener(RefreshCalendar);
            CalendarStore.AddStateChangeListener(UpdateView);
            CalendarStore.Initialize();
        }
    }
}
