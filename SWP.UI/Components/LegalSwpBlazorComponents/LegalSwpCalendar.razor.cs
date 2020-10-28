using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
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

        [Inject]
        public GeneralViewModel Gvm { get; set; }
    }
}
