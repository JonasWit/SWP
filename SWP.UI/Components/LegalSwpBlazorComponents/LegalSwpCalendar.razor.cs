using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

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
