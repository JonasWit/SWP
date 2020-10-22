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

        public ErrorPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public Task DisplayMessageAsync(Exception ex)
        {
            Exception = ex;
            App.SetActivePanel(LegalBlazorApp.Panels.ErrorPage);
            RefreshApp();
            return CreateLog(App.User.UserName, ex.Message, ex.StackTrace);
        }

        public void DisplayMessage(Exception ex)
        {
            Exception = ex;
            App.SetActivePanel(LegalBlazorApp.Panels.ErrorPage);
            RefreshApp();
            CreateLog(App.User.UserName, ex.Message, ex.StackTrace);
        }

        public void DismissMessage()
        {
            App.SetActivePanel(LegalBlazorApp.Panels.MyApp);
            RefreshApp();
        }

        public Task ThrowTestException()
        {
            try
            {
                throw new Exception("bablabla");
            }
            catch (Exception ex)
            {
                return DisplayMessageAsync(ex);
            }
        }

        public void RefreshApp() => App.ForceRefresh();
    }
}
