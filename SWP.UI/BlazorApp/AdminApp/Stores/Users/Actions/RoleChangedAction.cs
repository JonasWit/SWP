using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class RoleChangedAction : IAction
    {
        public const string RoleChanged = "ROLE_CHNAGED";
        public string Name => RoleChanged;

        public int Arg { get; set; }
    }
}
