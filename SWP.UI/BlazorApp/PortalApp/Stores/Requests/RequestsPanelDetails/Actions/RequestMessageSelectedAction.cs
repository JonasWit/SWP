using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelDetails.Actions
{
    public class RequestMessageSelectedAction : IAction
    {
        public const string RequestMessageSelected = "REQUEST_MESSAGE_SELECTED";
        public string Name => RequestMessageSelected;

        public int Arg { get; set; }
    }
}
