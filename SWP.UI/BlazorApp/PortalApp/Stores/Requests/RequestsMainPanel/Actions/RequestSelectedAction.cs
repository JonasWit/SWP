using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions
{
    public class RequestSelectedAction : IAction
    {
        public const string RequestSelected = "REQUEST_SELECTED";
        public string Name => RequestSelected;

        public RequestViewModel Arg { get; set; }
    }
}
