using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions
{
    public class AdminRequestSelectedChangeAction : IAction
    {
        public const string AdminCommRequestSelectedChange = "ADMIN_COMM_REQUEST_SELECTED_CHANGE";
        public string Name => AdminCommRequestSelectedChange;

        public RequestViewModel Arg { get; set; }
    }
}
