using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class SaveCaseRowAction : IAction
    {
        public const string SaveCaseRow = "SAVE_CASE_ROW";
        public string Name => SaveCaseRow;

        public CaseViewModel Arg { get; set; }
    }
}
