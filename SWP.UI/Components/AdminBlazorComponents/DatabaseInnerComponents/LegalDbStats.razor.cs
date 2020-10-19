using Microsoft.AspNetCore.Components;
using SWP.Application.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.DatabaseInnerComponents
{
    public partial class LegalDbStats
    {
        [Inject]
        public GetStatistics GetStatistics { get; set; }
        public ProfileFilterRecord SelectedProfile { get; set; }
        public List<ProfileFilterRecord> ProfileFilterRecords { get; set; } = new List<ProfileFilterRecord>();

        public class ProfileFilterRecord
        {
            public int Id { get; set; }
            public string Profile { get; set; }
        }

        public class ProfileStats
        {
            public int CasesCount { get; set; }
            public int CustomersCount { get; set; }
            public int ClientJobsCount { get; set; }
            public int ClientContactsCount { get; set; }
        }

        protected override void OnInitialized()
        {
            GetAllProfiles();
        }

        public void GetStatsForProfile()
        { 
            
        
        
        
        }

        public void GetAllProfiles()
        {
            var profiles = GetStatistics.GetProfiles();
            int counter = 0;

            foreach (var profile in profiles)
            {
                ProfileFilterRecords.Add(new ProfileFilterRecord
                {
                    Id = counter++,
                    Profile = profile
                });
            }
        }

        public void SelectedProfileChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                SelectedProfile = ProfileFilterRecords.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                SelectedProfile = null;
            }
        }
    }
}
