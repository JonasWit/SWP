using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class SaveLicenseRowAction : IAction
    {
        public const string SaveLicenseRow = "SAVE_LICENSE_ROW";
        public string Name => SaveLicenseRow;

        public UserLicense Arg { get; set; }
    }
}
