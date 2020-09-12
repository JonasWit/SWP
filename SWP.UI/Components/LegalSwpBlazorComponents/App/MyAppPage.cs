using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class MyAppPage : BlazorPageBase
    {
        private readonly ProfileStatistics profileStatistics;

        public LegalBlazorApp App { get; private set; }
        public StatisticsViewModel Statistics { get; private set; }

        public MyAppPage(ProfileStatistics profileStatistics)
        {
            this.profileStatistics = profileStatistics;
        }

        public override Task Initialize(BlazorAppBase app)
        { 
            App = app as LegalBlazorApp;
            RefreshStatistics();
            return Task.CompletedTask;
        }
     
        public void RefreshStatistics()
        {
            var test = profileStatistics.GetStatistics(App.User.Profile);





        }



    }
}
