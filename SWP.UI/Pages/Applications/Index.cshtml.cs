using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.UI.Models;

namespace SWP.UI.Pages.Applications
{
    [Authorize(Roles = "Users, Administrators")]
    public class IndexModel : PageModel
    {
        public string ActiveUserId { get; set; }
        public UserModel UserData { get; set; } = new UserModel();
        public List<string> Licenses { get; set; } = new List<string>();

        public IndexModel([FromServices] IHttpContextAccessor httpContextAccessor) =>
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<IActionResult> OnGet([FromServices] UserManager<IdentityUser> userManager)
        {
            UserData.User = await userManager.FindByIdAsync(ActiveUserId);
            UserData.Claims = await userManager.GetClaimsAsync(UserData.User) as List<Claim>; 

            foreach (var appClaim in UserData.Claims)
            {
                if (appClaim.Type == "Application")
                {
                    switch (appClaim.Value)
                    {
                        case "LegalSwp":
                            Licenses.Add("Aplikacja Kancelaria");
                            break;
                        case "MedicalSwp":
                            Licenses.Add("Aplikacja Gabinet");
                            break;
                        default:
                            break;
                    }
                }
            }

            return Page();
        }
    }
}
