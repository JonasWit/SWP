using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class EditClientRowAction : IAction
    {
        public const string EditClientRow = "EDIT_CLIENT_ROW";
        public string Name => EditClientRow;

        public ClientViewModel Arg { get; set; }
    }
}
