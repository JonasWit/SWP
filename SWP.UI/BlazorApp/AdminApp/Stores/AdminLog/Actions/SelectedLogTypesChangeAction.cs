using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class SelectedLogTypesChangeAction : IAction
    {
        public const string SelectedLogTypesChange = "SELECTED_LOG_TYPES_CHANGE";
        public string Name => SelectedLogTypesChange;

        public object Arg { get; set; }
    }
}
