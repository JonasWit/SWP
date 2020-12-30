using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.UserManager.Actions
{
    public class ConfirmRemoveAllDataAction : IAction
    {
        public const string ConfirmRemoveAllData = "CONFIRM_REMOVE_ALL_DATA";
        public string Name => ConfirmRemoveAllData;
    }
}
