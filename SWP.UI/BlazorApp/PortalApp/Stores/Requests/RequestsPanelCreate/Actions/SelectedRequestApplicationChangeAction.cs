using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions
{
    public class SelectedRequestApplicationChangeAction : IAction
    {
        public const string SelectedRequestApplicationChange = "SELECTED_REQUEST_APPLICATION_CHANGE";
        public string Name => SelectedRequestApplicationChange;

        public int? Arg { get; set; }
    }
}
