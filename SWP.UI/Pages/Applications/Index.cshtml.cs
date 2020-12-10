using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.UI.Models;
using SWP.UI.Pages.Applications.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Pages.Applications
{
    [Authorize(Roles = "Users, Administrators")]
    public class IndexModel : PageModel
    {
        public UserAccessModel AccessModel { get; set; } = new UserAccessModel();

        public class UserAccessModel
        {
            public string ActiveUserId { get; set; }
            public UserModel UserData { get; set; } = new UserModel();
            public List<LicenseViewModel> Licenses { get; set; } = new List<LicenseViewModel>();
            public LicenseViewModel LegalLicense => Licenses.FirstOrDefault(x => x.Application == ApplicationType.LegalSwp);
        }

        public IndexModel([FromServices] IHttpContextAccessor httpContextAccessor) =>
            AccessModel.ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<IActionResult> OnGet(
            [FromServices] UserManager<IdentityUser> userManager, 
            [FromServices] GetLicense getLicense, 
            [FromServices] IPortalManager portalManager)
        {
            AccessModel.UserData.User = await userManager.FindByIdAsync(AccessModel.ActiveUserId);
            AccessModel.UserData.Claims = await userManager.GetClaimsAsync(AccessModel.UserData.User) as List<Claim>;
            AccessModel.Licenses = getLicense.GetAll(AccessModel.ActiveUserId).Select(x => (LicenseViewModel)x).ToList();

            if (AccessModel.UserData.RootClient)
            {
                AccessModel.Licenses = getLicense.GetAll(AccessModel.ActiveUserId)
                    .Select(x => (LicenseViewModel)x).ToList();
            }
            else
            {
                var parentId = await portalManager.GetParentAccountId(AccessModel.UserData.User, AccessModel.UserData.ProfileClaim);

                if (!string.IsNullOrEmpty(parentId))
                {
                    AccessModel.Licenses = getLicense.GetAll(parentId).Select(x => (LicenseViewModel)x)?.ToList();
                }
            }

            return Page();
        }
    }
}
