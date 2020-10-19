using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Statistics
{
    [TransientService]
    public class GetStatistics
    {
        private readonly IStatisticsManager statisticsManager;
        public GetStatistics(IStatisticsManager statisticsManager) => this.statisticsManager = statisticsManager;

        public List<string> GetProfiles() => statisticsManager.GetProfiles();
        public int CountClients(string profile) => statisticsManager.CountClients(profile);

    }
}
