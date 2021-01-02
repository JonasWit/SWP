using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Pages.Applications.LegalApplication
{
    public partial class LegalApplicationHost : IDisposable
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool AccessGranted { get; set; } 

        public string ActiveUserId { get; set; } 

        public void Dispose()
        {
 
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            ActiveUserId = authState.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            AccessGranted = true;





        }
    }
}
