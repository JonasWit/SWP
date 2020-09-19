using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data.Statistics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class MyAppPage : BlazorPageBase
    {
        private readonly ProfileStatistics profileStatistics;

        public LegalBlazorApp App { get; private set; }

        public List<DataItem> CustomersCases { get; set; } = new List<DataItem>();

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
            var statistics = profileStatistics.GetStatistics(App.User.Profile);

            foreach (var customer in statistics.Customers)
            {
                CustomersCases.Add(new DataItem 
                { 
                    Category = customer.Name,
                    Number = customer.Cases.Count
                });
            }
        }
    }
}
