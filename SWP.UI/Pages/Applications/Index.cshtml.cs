using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Enums;
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
        public string ActiveUserId { get; set; }
        public UserModel UserData { get; set; } = new UserModel();
        public List<LicenseViewModel> Licenses { get; set; } = new List<LicenseViewModel>();
        public LicenseViewModel LegalLicense => Licenses.FirstOrDefault(x => x.Application == ApplicationType.LegalSwp);
        public string Profile => UserData.Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;

        public IndexModel([FromServices] IHttpContextAccessor httpContextAccessor) =>
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<IActionResult> OnGet([FromServices] UserManager<IdentityUser> userManager, [FromServices] GetLicense getLicense)
        {
            UserData.User = await userManager.FindByIdAsync(ActiveUserId);
            UserData.Claims = await userManager.GetClaimsAsync(UserData.User) as List<Claim>;
            Licenses = getLicense.GetAll(ActiveUserId).Select(x => (LicenseViewModel)x).ToList();

            return Page();
        }
    }
}
