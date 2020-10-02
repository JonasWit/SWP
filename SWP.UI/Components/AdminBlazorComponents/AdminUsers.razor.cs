﻿using Microsoft.AspNetCore.Components;
using SWP.Domain.Enums;
using SWP.UI.Components.AdminBlazorComponents.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents
{
    public partial class AdminUsers
    {
        [Parameter]
        public AdminBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<AdminBlazorApp> AppChanged { get; set; }

        private RoleType filteredRole;
    }
}
