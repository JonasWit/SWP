using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class CancelClientEditAction : IAction
    {
        public const string CancelClientEdit = "CANCEL_CLIENT_EDIT";
        public string Name => CancelClientEdit;

        public ClientViewModel Arg { get; set; }
    }
}
