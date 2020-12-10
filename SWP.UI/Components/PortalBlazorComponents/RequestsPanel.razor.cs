using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests;
using SWP.UI.Pages.PagesEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents
{
    public partial class RequestsPanel : IDisposable
    {
        [Inject]
        public RequestsStore Store { get; set; }
        [Parameter]
        public string ActiveUserId { get; set; }
        [Parameter]
        public RequestPreset Preset { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            Store.AddStateChangeListener(UpdateView);
        }


    }
}
