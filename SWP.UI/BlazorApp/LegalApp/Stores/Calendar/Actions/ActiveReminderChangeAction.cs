using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Calendar.Actions
{
    public class ActiveReminderChangeAction : IAction
    {
        public const string ActiveReminderChange = "ACTIVE_REMINDER_CHANGE";
        public string Name => ActiveReminderChange;
        public object Reminder { get; set; }
    }
}
