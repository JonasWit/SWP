using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class OnAppointmentRenderAction : IAction
    {
        public const string OnAppointmentRender = "ON_APPOINTMENT_RENDER";
        public string Name => OnAppointmentRender;

        public SchedulerAppointmentRenderEventArgs<ReminderViewModel> Arg { get; set; }
    }
}
