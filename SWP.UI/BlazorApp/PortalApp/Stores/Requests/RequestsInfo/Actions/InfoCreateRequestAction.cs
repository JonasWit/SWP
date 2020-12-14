using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsInfo.Actions
{
    public class InfoCreateRequestAction : IAction
    {
        public const string InfoCreateRequest = "INFO_CREATE_REQUEST";
        public string Name => InfoCreateRequest;
    }
}
