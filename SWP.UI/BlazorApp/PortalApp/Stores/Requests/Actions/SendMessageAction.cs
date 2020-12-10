using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.Actions
{
    public class SendMessageAction : IAction
    {
        public const string SendMessage = "SEND_MESSAGE";
        public string Name => SendMessage;
    }
}
