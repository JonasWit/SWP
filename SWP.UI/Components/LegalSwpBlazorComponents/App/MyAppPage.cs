using Radzen.Blazor;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class MyAppPage : BlazorPageBase
    {
        private readonly ProfileStatistics profileStatistics;

        public LegalBlazorApp App { get; private set; }

        public List<DataItem> ClientsCases { get; set; } = new List<DataItem>();
        public List<DataItem> UserActivity { get; set; } = new List<DataItem>();

        public ColorScheme ColorScheme { get; set; } = ColorScheme.Palette;

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
            RefreshClientCases();

        }

        private void  RefreshClientCases()
        {
            var statistics = profileStatistics.GetStatistics(App.User.Profile);

            foreach (var Client in statistics.Clients)
            {
                ClientsCases.Add(new DataItem
                {
                    Category = Client.Name,
                    Number = Client.Cases.Count
                });
            }
        }

        private void RefreshUsersActivity()
        {
            var statistics = profileStatistics.GetStatistics(App.User.Profile);

            foreach (var Client in statistics.Clients)
            {
                ClientsCases.Add(new DataItem
                {
                    Category = Client.Name,
                    Number = Client.Cases.Count
                });
            }
        }


    }
}
