using SWP.Application.LegalSwp.CashMovements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class SubmitNewCashMovementAction : IAction
    {
        public const string SubmitNewCashMovement = "SUBMIT_NEW_CASH_MOVEMENT";
        public string Name => SubmitNewCashMovement;

        public CreateCashMovement.Request Arg { get; set; }
    }
}
