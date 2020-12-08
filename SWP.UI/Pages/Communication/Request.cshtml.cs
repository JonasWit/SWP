using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.UI.Pages.PagesEnums;

namespace SWP.UI.Pages.Communication
{
    public class RequestModel : PageModel
    {
        public void OnGet(string requestedAction)
        {
            //todo: kupno - jak konto nie jest ani root ani powiazane to preset dla glownego
            //todo: kupno jak konto jest glowny to preset dla powiazanego do dodania

            if (Enum.TryParse(requestedAction, out RequestPreset preset))
            {





            }
            else
            { 
            
            
            }
        }
    }
}
