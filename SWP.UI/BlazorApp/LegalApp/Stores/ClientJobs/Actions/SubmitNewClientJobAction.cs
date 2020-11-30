using SWP.Application.LegalSwp.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class SubmitNewClientJobAction : IAction
    {
        public const string SubmitNewClientJob = "SUBMIT_NEW_CLIENT_JOB";
        public string Name => SubmitNewClientJob;

        public CreateClientJob.Request Arg { get; set; }
    }
}
