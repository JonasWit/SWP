using SWP.Application.PortalCustomers.RequestsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions
{
    public class CreateNewRequestAction : IAction
    {
        public const string CreateNewRequest = "CREATE_NEW_REQUEST";
        public string Name => CreateNewRequest;

        public CreateRequest.Request Arg { get; set; }
    }
}
