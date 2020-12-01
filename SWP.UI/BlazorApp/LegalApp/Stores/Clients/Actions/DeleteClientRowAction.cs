using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class DeleteClientRowAction : IAction
    {
        public const string DeleteClientRow = "DELETE_CLIENT_ROW";
        public string Name => DeleteClientRow;

        public ClientViewModel Arg { get; set; }
    }
}
