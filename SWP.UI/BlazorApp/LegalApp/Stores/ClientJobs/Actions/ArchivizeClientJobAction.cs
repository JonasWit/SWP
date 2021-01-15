using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class ArchivizeClientJobAction : IAction
    {
        public const string ArchivizeClientJob = "ARCHIVIZE_CLIENT_JOB";
        public string Name => ArchivizeClientJob;

        public ClientJobViewModel Arg { get; set; }
    }
}
