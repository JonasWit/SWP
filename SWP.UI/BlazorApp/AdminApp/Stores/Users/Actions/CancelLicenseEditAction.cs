using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class CancelLicenseEditAction : IAction
    {
        public const string CancelLicenseEdit = "CANCEL_LICENSE_ROW";
        public string Name => CancelLicenseEdit;

        public UserLicense Arg { get; set; }
    }
}
