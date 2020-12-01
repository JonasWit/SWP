using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MyApp.Actions
{
    public class SelectedUserChangeAction : IAction
    {
        public const string SelectedUserChange = "SELECTED_USER_CHANGE";
        public string Name => SelectedUserChange;

        public object Arg { get; set; }
    }
}
