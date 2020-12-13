using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.Actions
{
    public class RequestsPanelSendMessageAction : IAction
    {
        public const string SendMessage = "REQUEST_PANEL_SEND_MESSAGE";
        public string Name => SendMessage;
    }
}
