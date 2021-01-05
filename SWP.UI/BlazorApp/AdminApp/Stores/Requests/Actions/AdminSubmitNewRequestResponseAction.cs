using SWP.Application.PortalCustomers.RequestsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions
{
    public class AdminSubmitNewRequestResponseAction : IAction
    {
        public const string AdminSubmitNewRequestResponse = "ADMIN_SUBMIT_NEW_REQUEST_RESPONSE";
        public string Name => AdminSubmitNewRequestResponse;

        public CreateRequest.RequestMessage Arg { get; set; }
    }
}
