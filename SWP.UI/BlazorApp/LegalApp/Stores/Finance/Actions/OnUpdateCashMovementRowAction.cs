using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class OnUpdateCashMovementRowAction : IAction
    {
        public const string OnUpdateCashMovementRow = "ON_UPDATE_CASH_MOVEMENT_ROW";
        public string Name => OnUpdateCashMovementRow;

        public CashMovementViewModel Arg { get; set; }
    }
}
