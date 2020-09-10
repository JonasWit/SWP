using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class ErrorPage
    {
        public Exception Exception { get; private set; }

        public LegalSwpApp App { get; private set; }

        public void Initialize(LegalSwpApp app) => App = app;

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
