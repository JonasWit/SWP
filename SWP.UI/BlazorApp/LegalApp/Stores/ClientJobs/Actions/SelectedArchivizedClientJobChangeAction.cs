using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class SelectedArchivizedClientJobChangeAction : IAction
    {
        public const string SelectedArchivizedClientJobChange = "SELECTED_ARCHIVIZED_CLIENT_JOB";
        public string Name => SelectedArchivizedClientJobChange;

        public object Arg { get; set; }
    }
}
