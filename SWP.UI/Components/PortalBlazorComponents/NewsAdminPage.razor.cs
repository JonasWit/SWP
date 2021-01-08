using Microsoft.AspNetCore.Components;
using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents
{
    public partial class NewsAdminPage : IDisposable
    {
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
         
        }

        private void UpdateView() => StateHasChanged();

        protected override void OnInitialized()
        {
        
 
        }

        #region Actions










        #endregion


    }
}
