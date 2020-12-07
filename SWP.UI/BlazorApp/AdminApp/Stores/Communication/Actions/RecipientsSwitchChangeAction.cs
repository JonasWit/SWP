using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions
{
    public class RecipientsSwitchChangeAction : IAction
    {
        public const string RecipientsSwitchChange = "RECIPIENT_SWITCH_CHANGE";
        public string Name => RecipientsSwitchChange;
    }
}
