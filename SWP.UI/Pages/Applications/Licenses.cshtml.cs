using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SWP.UI.Pages.Applications
{
    [Authorize(Roles = "Users, Administrators")]
    public class LicensesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
