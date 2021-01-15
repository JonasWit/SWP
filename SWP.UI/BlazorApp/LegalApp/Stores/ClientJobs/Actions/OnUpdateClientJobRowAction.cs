using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class OnUpdateClientJobRowAction : IAction
    {
        public const string OnUpdateClientJobRow = "ON_UPDATE_CLIENT_JOB_ROW";
        public string Name => OnUpdateClientJobRow;

        public ClientJobViewModel Arg { get; set; }
    }
}
