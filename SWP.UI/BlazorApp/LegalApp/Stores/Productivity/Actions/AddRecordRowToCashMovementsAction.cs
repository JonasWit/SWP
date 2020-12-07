using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class AddRecordRowToCashMovementsAction : IAction
    {
        public const string AddRecordRowToCashMovements = "ADD_RECORD_TO_CASH_MOVEMENTS";
        public string Name => AddRecordRowToCashMovements;

        public TimeRecordViewModel Arg { get; set; }
    }
}
