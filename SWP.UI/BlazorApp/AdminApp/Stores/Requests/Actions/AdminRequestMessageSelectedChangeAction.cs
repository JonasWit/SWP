using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions
{
    public class AdminRequestMessageSelectedChangeAction : IAction
    {
        public const string AdminRequestMessageSelectedChange = "ADMIN_COMM_REQUEST_MESSAGE_SELECTED_CHANGE";
        public string Name => AdminRequestMessageSelectedChange;

        public int Arg { get; set; }
    }
}
