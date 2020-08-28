using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class MyAppPanel
    {
        private readonly ProfileStatistics profileStatistics;

        public LegalSwpApp App { get; private set; }
        public StatisticsViewModel Statistics { get; private set; }

        public MyAppPanel(ProfileStatistics profileStatistics)
        {
            this.profileStatistics = profileStatistics;
        }

        public void Initialize(LegalSwpApp app)
        { 
            App = app;
            RefreshStatistics();
        }

     
        public void RefreshStatistics()
        {
            var test = profileStatistics.GetStatistics(App.User.Profile);





        }



    }
}
