using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class SaveClientJobRowAction : IAction
    {
        public const string SaveClientJobRow = "SAVE_CLIENT_JOB_ROW";
        public string Name => SaveClientJobRow;

        public ClientJobViewModel Arg { get; set; }
    }
}
