using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions
{
    public class UserManagerUpdateAccessAction : IAction
    {
        public const string UserManagerUpdateAccess = "USER_MANAGER_UPDATE_ACCESS";
        public string Name => UserManagerUpdateAccess;
    }
}
