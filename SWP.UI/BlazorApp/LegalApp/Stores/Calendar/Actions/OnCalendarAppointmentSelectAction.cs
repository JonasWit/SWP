using Radzen;
using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Calendar.Actions
{
    public class OnCalendarAppointmentSelectAction: IAction
    {
        public const string OnAppointmentSelect = "ON_CALENDAT_APPOINTMENT_SELECT";
        public string Name => OnAppointmentSelect;
        public SchedulerAppointmentSelectEventArgs<ReminderViewModel> Args{ get; set; }
    }
}
