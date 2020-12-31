using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class SelectedMonthChangeFinanceAction : IAction
    {
        public const string SelectedMonthChange = "SELECTED_MONTH_CHANGE_FINANCE";
        public string Name => SelectedMonthChange;

        public object Arg { get; set; }
    }
}
