using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class LogRowSelectedAction : IAction
    {
        public const string LogRowSelected = "LOG_ROW_SELECTED";
        public string Name => LogRowSelected;

        public object Arg { get; set; }
    }
}
