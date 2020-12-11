using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SWP.UI.Pages.Communication
{
    [Authorize(Roles = "Users, Administrators")]
    public class RequestModel : PageModel
    {
        public string ActiveUserId { get; set; }

        public void OnGet([FromServices] IHttpContextAccessor httpContextAccessor, string requestedAction = null)
        {
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


        }
    }
}
