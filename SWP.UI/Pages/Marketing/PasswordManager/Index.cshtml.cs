using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SWP.UI.Pages.Marketing.PasswordManager
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnGetDownloadFile() => File("/downloadable-files/PasswordManager.exe", MediaTypeNames.Application.Octet, "PasswordManager.exe");
    }
}
