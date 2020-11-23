using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class SelectedArchivizedCaseChangeAction : IAction
    {
        public const string SelectedArchivizedCaseChange = "SELECTED_ARCHIVIZED_CASE_CHANGE";
        public string Name => SelectedArchivizedCaseChange;
        public object Case { get; set; }
    }
}
