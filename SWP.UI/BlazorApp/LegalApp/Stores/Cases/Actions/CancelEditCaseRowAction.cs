using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class CancelEditCaseRowAction : IAction
    {
        public const string CancelEditCaseRow = "CANCEL_EDIT_CASE_ROW";
        public string Name => CancelEditCaseRow;

        public CaseViewModel Arg { get; set; }
    }
}
