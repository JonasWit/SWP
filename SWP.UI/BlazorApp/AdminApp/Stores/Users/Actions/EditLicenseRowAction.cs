using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class EditLicenseRowAction : IAction
    {
        public const string EditLicenseRow = "EDIT_LICENSE_ROW";
        public string Name => EditLicenseRow;

        public UserLicense Arg { get; set; }
    }
}
