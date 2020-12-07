using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class LogStartDateChangeAction : IAction
    {
        public const string LogStartDateChange = "LOG_START_DATE_CHANGE";
        public string Name => LogStartDateChange;

        public object Arg { get; set; }
    }
}
