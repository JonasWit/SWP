using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class AddApplicationClaimAction : IAction
    {
        public const string AddApplicationClaim = "ADD_APPLICATION_CLAIM";
        public string Name => AddApplicationClaim;
    }
}
