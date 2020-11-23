using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class DeleteSelectedClientAction : IAction
    {
        public const string DeleteSelectedClient = "DELETE_SELECTED_CLIENT";
        public string Name => DeleteSelectedClient;
    }
}
