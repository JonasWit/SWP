using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class UserRowSelectedAction : IAction
    {
        public const string UserRowSelected = "USER_ROW_SELECTED";
        public string Name => UserRowSelected;

        public object Arg { get; set; }
    }
}
