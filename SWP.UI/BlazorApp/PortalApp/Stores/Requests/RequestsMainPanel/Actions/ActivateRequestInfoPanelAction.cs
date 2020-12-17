using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions
{
    public class ActivateRequestInfoPanelAction : IAction
    {
        public const string ActivateRequestInfoPanel = "ACTIVATE_REQUEST_INFO_PANEL";
        public string Name => ActivateRequestInfoPanel;
    }
}
