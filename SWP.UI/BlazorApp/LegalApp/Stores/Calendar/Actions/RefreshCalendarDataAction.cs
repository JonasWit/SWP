using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Calendar.Actions
{
    public class RefreshCalendarDataAction : IAction
    {
        public const string RefreshCalendarData = "REFRESH_CALENDAR_DATA";
        public string Name => RefreshCalendarData;
    }
}
