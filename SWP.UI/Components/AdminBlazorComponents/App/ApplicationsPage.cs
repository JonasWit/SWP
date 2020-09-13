using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class ApplicationsPage : BlazorPageBase
    {
        public AdminBlazorApp App { get; set; }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;
            return Task.CompletedTask;
        }

    }
}
