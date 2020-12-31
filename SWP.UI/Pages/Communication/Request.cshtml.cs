using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.UI.Utilities;
using System.Security.Claims;

namespace SWP.UI.Pages.Communication
{
    [Authorize(Policy = PortalNames.Policies.AuthenticatedUser)]
    public class RequestModel : PageModel
    {
        public string ActiveUserId { get; set; }
        public string ActiveUserName { get; set; }

        public void OnGet([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ActiveUserName = httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
