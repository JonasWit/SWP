using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class ArchivizeClientAction : IAction
    {
        public const string ArchivizeClient = "ARCHIVIZE_CLIENT";
        public string Name => ArchivizeClient;

        public ClientViewModel Arg { get; set; }
    }
}
