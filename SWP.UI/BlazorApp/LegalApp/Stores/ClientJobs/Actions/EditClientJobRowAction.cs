using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions
{
    public class EditClientJobRowAction : IAction
    {
        public const string EditClientJobRow = "EDIT_CLIENT_JOB_ROW";
        public string Name => EditClientJobRow;

        public ClientJobViewModel Arg { get; set; }
    }
}
