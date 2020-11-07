using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Error;
using SWP.UI.Components.AdminBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminError
    {
        [Inject]
        public ApplicationStore ApplicationStore { get; set; }
        [Inject]
        public ErrorStore ErrorStore { get; set; }

        public void Dispose()
        {
            ApplicationStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ApplicationStore.AddStateChangeListener(UpdateView);
        }
    }
}
