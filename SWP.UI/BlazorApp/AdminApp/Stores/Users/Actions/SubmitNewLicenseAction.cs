using SWP.Application.PortalCustomers.LicenseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class SubmitNewLicenseAction : IAction
    {
        public const string SubmitNewLicense = "SUBMIT_NEW_LICENSE";
        public string Name => SubmitNewLicense;

        public CreateLicense.Request Arg { get; set; }
    }
}
