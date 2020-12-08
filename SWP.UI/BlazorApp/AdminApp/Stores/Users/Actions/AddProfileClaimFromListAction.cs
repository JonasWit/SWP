using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class AddProfileClaimFromListAction : IAction
    {
        public const string AddProfileClaimFromList = "ADD_PROFILE_CLAIM_FROM_LIST";
        public string Name => AddProfileClaimFromList;
    }
}
