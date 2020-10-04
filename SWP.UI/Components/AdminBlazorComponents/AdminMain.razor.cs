using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminMain : IDisposable
    {
        [Parameter]
        public string ActiveUserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await App.Initialize(ActiveUserId);
            App.CallStateHasChanged += new EventHandler(CallStateHasChanged);
        }

        public void CallStateHasChanged(object sender, EventArgs e) => StateHasChanged();

        public void Dispose() => App.CallStateHasChanged -= new EventHandler(CallStateHasChanged);

        private bool hamburgerNavActive;

        private void ActivateNavBurger() => hamburgerNavActive = !hamburgerNavActive;
    }
}
