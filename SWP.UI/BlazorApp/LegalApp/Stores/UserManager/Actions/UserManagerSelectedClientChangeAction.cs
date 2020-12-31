using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions
{
    public class UserManagerSelectedClientChangeAction : IAction
    {
        public const string UserManagerSelectedClientChange = "USER_MANAGER_SELECTED_CLIENT_CHANGE";
        public string Name => UserManagerSelectedClientChange;

        public object Arg { get; set; }
    }
}
