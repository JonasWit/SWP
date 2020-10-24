using SWP.Domain.Infrastructure.LegalApp;
using System.Collections.Generic;

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
