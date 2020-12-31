using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions
{
    public class UserManagerSelectedCaseChangeAction : IAction
    {
        public const string UserManagerSelectedCaseChange = "USER_MANAGER_SELECTED_CASE_CHANGE";
        public string Name => UserManagerSelectedCaseChange;

        public object Arg { get; set; }
    }
}
