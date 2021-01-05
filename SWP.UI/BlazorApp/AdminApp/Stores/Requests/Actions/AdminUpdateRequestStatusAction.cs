using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions
{
    public class AdminUpdateRequestStatusAction : IAction
    {
        public const string AdminUpdateRequestStatus = "ADMIN_UPDATE_REQUEST_STATUS";
        public string Name => AdminUpdateRequestStatus;
    }
}
