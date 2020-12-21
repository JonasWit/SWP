using SWP.Application.PortalCustomers.RequestsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelDetails.Actions
{
    public class CreateNewRequestMessageAction : IAction
    {
        public const string CreateNewRequestMessage = "CREATE_NEW_REQUEST_MESSAGE";
        public string Name => CreateNewRequestMessage;

        public CreateRequest.RequestMessage Arg { get; set; }
    }
}
