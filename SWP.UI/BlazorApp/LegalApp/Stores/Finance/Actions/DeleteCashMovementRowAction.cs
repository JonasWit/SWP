using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class DeleteCashMovementRowAction : IAction
    {
        public const string DeleteCashMovementRow = "DELETE_CASH_MOVEMENT_ROW";
        public string Name => DeleteCashMovementRow;

        public CashMovementViewModel Arg { get; set; }
    }
}
