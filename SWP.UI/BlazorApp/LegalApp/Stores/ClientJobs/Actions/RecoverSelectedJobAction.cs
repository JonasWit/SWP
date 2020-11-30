using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class RecoverSelectedJobAction : IAction
    {
        public const string RecoverSelectedJob = "RECOVER_SELECTED_JOB";
        public string Name => RecoverSelectedJob;
    }
}
