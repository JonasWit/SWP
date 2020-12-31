using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class SelectedMonthChangeProductivityAction : IAction
    {
        public const string SelectedMonthChange = "SELECTED_MONTH_CHANGE_PRODUCTIVITY";
        public string Name => SelectedMonthChange;

        public object Arg { get; set; }
    }
}
