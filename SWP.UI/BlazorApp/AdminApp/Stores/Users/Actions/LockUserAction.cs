using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class LockUserAction : IAction
    {
        public const string LockUser = "LOCK_USER";
        public string Name => LockUser;

        public bool Arg { get; set; }
    }
}
