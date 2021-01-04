using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.UI.Models;
using SWP.UI.Pages.Applications.ViewModels;
using SWP.UI.Utilities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Pages.Applications
{
    [Authorize(Policy = PortalNames.Policies.AuthenticatedUser)]
    public class IndexModel : PageModel
    {
        public UserAccessModel AccessModel { get; set; } = new UserAccessModel();
        public ApplicationType ActiveApp { get; set; } = ApplicationType.NoApp;

        public class UserAccessModel
        {
            public string ActiveUserId { get; set; }
            public AppActiveUserManager AppActiveUserManager { get; set; }
            public LicenseViewModel LegalLicense => AppActiveUserManager.LicenseVms.FirstOrDefault(x => x.Application == ApplicationType.LegalApplication);
        }

        public IndexModel([FromServices] IHttpContextAccessor httpContextAccessor) =>
            AccessModel.ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<IActionResult> OnGet([FromServices] IServiceProvider serviceProvider)
        {
            AccessModel.AppActiveUserManager = new AppActiveUserManager(serviceProvider, AccessModel.ActiveUserId);
            await AccessModel.AppActiveUserManager.UpdateClaimsAndRoles();
            await AccessModel.AppActiveUserManager.UpdateLicenses();

            return Page();
        }

        public void OnGetLegalApp()
        {
            ActiveApp = ApplicationType.LegalApplication;
        }
    }
}
