using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SWP.UI.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnGetDownloadPasswordManager() => File("/DownloadableFiles/PasswordManager.exe", "application/octet-stream",
                "PasswordManager.exe");
    }
}
