using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class DeleteCaseRowAction : IAction
    {
        public const string DeleteCaseRow = "DELETE_CASE_ROW";
        public string Name => DeleteCaseRow;

        public CaseViewModel Arg { get; set; }
    }
}
