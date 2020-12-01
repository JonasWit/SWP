using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class SaveClientRowAction : IAction
    {
        public const string SaveClientRow = "SAVE_CLIENT_ROW";
        public string Name => SaveClientRow;

        public ClientViewModel Arg { get; set; }
    }
}
