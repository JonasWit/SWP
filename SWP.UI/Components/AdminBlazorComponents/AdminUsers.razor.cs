using Microsoft.AspNetCore.Components;
using SWP.UI.Components.AdminBlazorComponents.App;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminUsers
    {
        [Parameter]
        public AdminBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<AdminBlazorApp> AppChanged { get; set; }
    }
}
