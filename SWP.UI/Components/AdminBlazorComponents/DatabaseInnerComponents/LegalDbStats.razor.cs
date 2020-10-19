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
        public ProfileStats Statistics { get; set; }

        public class ProfileFilterRecord
        {
            public int Id { get; set; }
            public string Profile { get; set; }
        }

        public class ProfileStats
        {
            public int CasesCount { get; set; }
            public int ClientsCount { get; set; }
            public int ClientJobsCount { get; set; }
            public int ClientContactsCount { get; set; }
        }

        protected override void OnInitialized()
        {
            GetAllProfiles();
        }

        private void GetStatsForProfile()
        {
            Statistics = new ProfileStats
            {
                ClientsCount = GetStatistics.CountClients(SelectedProfile.Profile)
            };
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
                GetStatsForProfile();
            }
            else
            {
                SelectedProfile = null;
                Statistics = null;
            }
        }
    }
}
