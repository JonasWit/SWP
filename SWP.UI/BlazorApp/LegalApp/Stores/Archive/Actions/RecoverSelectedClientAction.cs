using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class RecoverSelectedClientAction : IAction
    {
        public const string RecoverSelectedClient = "RECOVER_SELECTED_CLIENT";
        public string Name => RecoverSelectedClient;
    }
}
