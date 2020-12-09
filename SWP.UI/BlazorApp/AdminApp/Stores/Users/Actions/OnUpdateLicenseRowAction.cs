using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class OnUpdateLicenseRowAction : IAction
    {
        public const string OnUpdateLicenseRow = "ON_UPDATE_LICENSE_ROW";
        public string Name => OnUpdateLicenseRow;

        public UserLicense Arg { get; set; }
    }
}
