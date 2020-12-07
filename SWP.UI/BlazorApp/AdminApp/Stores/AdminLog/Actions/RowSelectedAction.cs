using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class RowSelectedAction : IAction
    {
        public const string RowSelected = "ROW_SELECTED";
        public string Name => RowSelected;

        public object Arg { get; set; }
    }
}
