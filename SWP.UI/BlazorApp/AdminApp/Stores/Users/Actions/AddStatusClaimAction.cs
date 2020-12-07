using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class AddStatusClaimAction : IAction
    {
        public const string AddStatusClaim = "ADD_STATUS_CLAIM";
        public string Name => AddStatusClaim;
    }
}
