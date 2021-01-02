using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Domain.Enums;
using SWP.UI.Models;
using SWP.UI.Utilities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Pages.LegalSwp
{
    [Authorize(Policy = PortalNames.Policies.LegalApplication + ", " + PortalNames.Policies.AuthenticatedUser)]
    public class IndexModel : PageModel
    {
        public string ActiveUserId { get; set; }

        public async Task<IActionResult> OnGet(
            [FromServices] IHttpContextAccessor httpContextAccessor, 
            [FromServices] IServiceProvider serviceProvider)
        {
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var appActiveUserManager = new AppActiveUserManager(serviceProvider, ActiveUserId);
            await appActiveUserManager.UpdateClaimsAndRoles();
            await appActiveUserManager.UpdateLicenses();

            var legalLicense = appActiveUserManager.LicenseVms.FirstOrDefault(x => x.Application == ApplicationType.LegalApplication);

            if (appActiveUserManager.IsLocked || legalLicense is null || appActiveUserManager.ProfileClaim is null ||
                appActiveUserManager.LicenseVms.Count == 0 || legalLicense.ValidTo <= DateTime.Now)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }       
    }
}
