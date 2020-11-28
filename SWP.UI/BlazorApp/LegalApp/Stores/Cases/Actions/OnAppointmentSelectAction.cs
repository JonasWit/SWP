using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class OnAppointmentSelectAction : IAction
    {
        public const string OnAppointmentSelect = "ON_APPOINTMENT_SELECT";
        public string Name => OnAppointmentSelect;

        public SchedulerAppointmentSelectEventArgs<ReminderViewModel> Arg { get; set; }
    }
}
