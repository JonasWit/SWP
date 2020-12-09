using SWP.Domain.Enums;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Pages.Applications.ViewModels
{
    public class LicenseViewModel
    {
        public string UserId { get; set; }
        public ApplicationType Application { get; set; }
        public LicenseType Type { get; set; }
        public int RelatedUsers { get; set; }
        public DateTime ValidTo { get; set; }

        public static implicit operator LicenseViewModel(UserLicense input) =>
            new LicenseViewModel
            {
                UserId = input.UserId,
                Application = Enum.TryParse(input.Application, out ApplicationType appType) ? appType : ApplicationType.NoApp,
                Type = Enum.TryParse(input.Application, out LicenseType licenseType) ? licenseType : LicenseType.Trial,
                RelatedUsers = input.RelatedUsers,
                ValidTo = input.ValidTo
            };
    }
}
