using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.UI.Utilities;
using System.Security.Claims;

namespace SWP.UI.Pages.LegalSwp
{
    [Authorize(Policy = PortalNames.Policies.LegalApplication)]
    public class IndexModel : PageModel
    {
        public string ActiveUserId { get; set; }

        public void OnGet([FromServices] IHttpContextAccessor httpContextAccessor) => 
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
