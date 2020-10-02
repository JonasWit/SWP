using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class ErrorPage : BlazorPageBase
    {
        public Exception Exception { get; private set; }

        public AdminBlazorApp App { get; set; }

        public ErrorPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;
            return Task.CompletedTask;
        }

        public void DisplayMessage(Exception ex)
        {
            Exception = ex;
            App.SetActivePanel(AdminBlazorApp.Panels.Error);
            RefreshApp();
        }

        public void DismissMessage()
        {
            App.SetActivePanel(AdminBlazorApp.Panels.Error);
            RefreshApp();
        }

        public void RefreshApp() => App.ForceRefresh();

    }
}
