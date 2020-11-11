using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.LegalApp.Stores.Error;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpError
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ErrorStore ErrorStore { get; set; }
    }
}
