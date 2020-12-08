using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class SelectedProfileChangeAction : IAction
    {
        public const string SelectedProfileChange = "SELECTED_PROFILE_CHANGE";
        public string Name => SelectedProfileChange;

        public object Arg { get; set; }
    }
}
