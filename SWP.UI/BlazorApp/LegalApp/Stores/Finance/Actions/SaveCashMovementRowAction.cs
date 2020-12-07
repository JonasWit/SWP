using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class SaveCashMovementRowAction : IAction
    {
        public const string SaveCashMovementRow = "SAVE_CASH_MOVEMENT_ROW";
        public string Name => SaveCashMovementRow;

        public CashMovementViewModel Arg { get; set; }
    }
}
