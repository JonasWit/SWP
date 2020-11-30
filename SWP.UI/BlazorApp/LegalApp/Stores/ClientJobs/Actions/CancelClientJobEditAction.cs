using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class CancelClientJobEditAction : IAction
    {
        public const string CancelClientJobEdit = "CANCEL_CLIENT_JOB_EDIT";
        public string Name => CancelClientJobEdit;

        public ClientJobViewModel Arg { get; set; }
    }
}
