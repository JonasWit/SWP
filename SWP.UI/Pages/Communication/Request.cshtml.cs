using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.UI.Pages.PagesEnums;

namespace SWP.UI.Pages.Communication
{
    [Authorize(Roles = "Users, Administrators")]
    public class RequestModel : PageModel
    {
        public RequestPreset Preset { get; set; }
        public string ActiveUserId { get; set; }

        public void OnGet([FromServices] IHttpContextAccessor httpContextAccessor, string requestedAction = null)
        {
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (Enum.TryParse(requestedAction, out RequestPreset preset))
            {
                Preset = preset;
            }
            else
            {
                Preset = RequestPreset.Blank;
            }
        }
    }
}
