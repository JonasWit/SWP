using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class ActiveCashMovementChangeAction : IAction
    {
        public const string ActiveCashMovementChange = "ACTIVE_CASH_MOVEMENT_CHANGE";
        public string Name => ActiveCashMovementChange;

        public object Arg { get; set; }
    }
}
