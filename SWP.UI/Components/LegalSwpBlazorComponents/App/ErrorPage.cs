using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ErrorPage : BlazorPageBase
    {
        public Exception Exception { get; private set; }

        public LegalBlazorApp App { get; private set; }

        public override void Initialize(BlazorAppBase app) => App = app as LegalBlazorApp;

        public void DisplayMessage(Exception ex)
        {
            Exception = ex;
            App.SetActivePanel(LegalBlazorApp.Panels.ErrorPage);
            RefreshApp();
        }

        public void DismissMessage()
        {
            App.SetActivePanel(LegalBlazorApp.Panels.MyApp);
            RefreshApp();
        }

        public void RefreshApp() => App.ForceRefresh();
    }
}
