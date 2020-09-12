using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class ErrorPage : BlazorPageBase
    {
        public Exception Exception { get; private set; }

        public LegalSwpApp App { get; private set; }

        public override void Initialize(BlazorAppBase app) => App = app as LegalSwpApp;

        public void DisplayMessage(Exception ex)
        {
            Exception = ex;
            App.SetActivePanel(LegalSwpApp.Panels.ErrorPage);
            RefreshApp();
        }

        public void DismissMessage()
        {
            App.SetActivePanel(LegalSwpApp.Panels.MyApp);
            RefreshApp();
        }

        public void RefreshApp() => App.ForceRefresh();
    }
}
