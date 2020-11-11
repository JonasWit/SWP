using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Calendar;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Threading.Tasks;

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
            CalendarStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            CalendarStore.AddStateChangeListener(UpdateView);
            CalendarStore.Initialize();
        }
    }
}
