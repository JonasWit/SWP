using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpArchive
    {
        [Inject]
        public LegalBlazorApp App { get; set; }

        public string ArchvizedClientsFilterValue;

        protected override void OnInitialized()
        {
           App.ArchivePage.RefreshData();
        }
    }
}
