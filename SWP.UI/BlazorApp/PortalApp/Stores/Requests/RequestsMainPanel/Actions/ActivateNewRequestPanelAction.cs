using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions
{
    public class ActivateNewRequestPanelAction : IAction
    {
        public const string ActivateNewRequestPanel = "ACTIVATE_NEW_REQUEST_PANEL";
        public string Name => ActivateNewRequestPanel;
    }
}
