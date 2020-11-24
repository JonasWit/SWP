using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class EditCaseRowAction : IAction
    {
        public const string EditCaseRow = "EDIT_CASE_ROW_ACTION";
        public string Name => EditCaseRow;

        public CaseViewModel Arg { get; set; }
    }
}
