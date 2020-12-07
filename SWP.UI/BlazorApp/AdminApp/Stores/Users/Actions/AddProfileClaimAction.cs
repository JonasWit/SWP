using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class AddProfileClaimAction : IAction
    {
        public const string AddProfileClaim = "ADD_PROFILE_CLAIM";
        public string Name => AddProfileClaim;
    }
}
