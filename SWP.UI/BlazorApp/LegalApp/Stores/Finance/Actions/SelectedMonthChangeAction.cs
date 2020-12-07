using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class SelectedMonthChangeAction : IAction
    {
        public const string SelectedMonthChange = "SELECTED_MONTH_CHANGE";
        public string Name => SelectedMonthChange;

        public object Arg { get; set; }
    }
}
