using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpArchive
    {
        [Parameter]
        public LegalBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<LegalBlazorApp> AppChanged { get; set; }

        public string ArchvizedClientsFilterValue;

        protected override void OnInitialized()
        {
           App.ArchivePage.RefreshData();
        }
    }
}
