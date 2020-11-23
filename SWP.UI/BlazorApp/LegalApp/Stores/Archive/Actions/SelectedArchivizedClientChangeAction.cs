using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class SelectedArchivizedClientChangeAction : IAction
    {
        public const string SelectedArchivizedClientChange = "SELECTED_ARCHIVIZED_CLIENT_CHANGE";
        public string Name => SelectedArchivizedClientChange;
        public object Client { get; set; }
    }
}
