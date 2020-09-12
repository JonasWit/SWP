using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;


namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class MyAppPanel : BlazorPageBase
    {
        private readonly ProfileStatistics profileStatistics;

        public LegalSwpApp App { get; private set; }
        public StatisticsViewModel Statistics { get; private set; }

        public MyAppPanel(ProfileStatistics profileStatistics)
        {
            this.profileStatistics = profileStatistics;
        }

        public override void Initialize(BlazorAppBase app)
        { 
            App = app as LegalSwpApp;
            RefreshStatistics();
        }

     
        public void RefreshStatistics()
        {
            var test = profileStatistics.GetStatistics(App.User.Profile);





        }



    }
}
