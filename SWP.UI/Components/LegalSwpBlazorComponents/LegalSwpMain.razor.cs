using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMain
    {
        [Parameter]
        public string ActiveUserId { get; set; }
        [Inject]
        public LegalBlazorApp App { get; set; }

        public bool initializing = false;

        protected override async Task OnInitializedAsync()
        {
            initializing = true;

            await App.Initialize(ActiveUserId);
            App.CallStateHasChanged += new EventHandler(CallStateHasChanged);

            initializing = false;
        }

        public void CallStateHasChanged(object sender, EventArgs e) => StateHasChanged();

        public void Dispose() => App.CallStateHasChanged -= new EventHandler(CallStateHasChanged);
    }
}
