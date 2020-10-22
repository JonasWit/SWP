using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpCalendar
    {
        [Inject]
        public LegalBlazorApp App { get; set; }
        //[Parameter]
        //public EventCallback<LegalBlazorApp> AppChanged { get; set; }
    }
}
