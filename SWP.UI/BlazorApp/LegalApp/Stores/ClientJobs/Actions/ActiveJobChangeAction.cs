using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class ActiveJobChangeAction : IAction
    {
        public const string ActiveJobChange = "ACTIVE_JOB_CHANGE";
        public string Name => ActiveJobChange;

        public object Arg { get; set; }
    }
}
