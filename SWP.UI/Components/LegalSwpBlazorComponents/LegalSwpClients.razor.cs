﻿using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClients
    {
        [Parameter]
        public LegalBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<LegalBlazorApp> AppChanged { get; set; }

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

        public bool clientListInfoVisible = false;
        public void ShowHideClientI() => clientListInfoVisible = !clientListInfoVisible;



    }
}