using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class ArchivizeCaseAction : IAction
    {
        public const string ArchivizeCase = "ARCHIVIZE_CASE";
        public string Name => ArchivizeCase;

        public CaseViewModel Arg { get; set; }
    }
}
