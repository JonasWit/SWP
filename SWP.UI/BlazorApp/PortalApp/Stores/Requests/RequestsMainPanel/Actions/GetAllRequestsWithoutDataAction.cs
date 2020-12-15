using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsMainPanel.Actions
{
    public class GetAllRequestsWithoutDataAction : IAction
    {
        public const string GetAllRequestsWithoutData = "GET_ALL_REQUESTS_WITOHUT_DATA";
        public string Name => GetAllRequestsWithoutData;
    }
}
