using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class DeleteClientJobRowAction : IAction
    {
        public const string DeleteClientJobRow = "DELETE_CLIENT_JOB_ROW";
        public string Name => DeleteClientJobRow;

        public ClientJobViewModel Arg { get; set; }
    }
}
