using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class OnUpdateClientRowAction : IAction
    {
        public const string OnUpdateClientRow = "ON_UPDATE_CLIENY_ROW";
        public string Name => OnUpdateClientRow;

        public ClientViewModel Arg { get; set; }
    }
}
