using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class RefreshLogsForLastWeekAction : IAction
    {
        public const string RefreshLogsForLastWeek = "REFRESH_LOGS_LAST_WEEK";
        public string Name => RefreshLogsForLastWeek;
    }
}
