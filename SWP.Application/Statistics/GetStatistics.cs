using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.Statistics
{
    [TransientService]
    public class GetStatistics
    {
        private readonly IStatisticsManager statisticsManager;
        public GetStatistics(IStatisticsManager statisticsManager) => this.statisticsManager = statisticsManager;

        public List<string> GetProfiles() => statisticsManager.GetProfiles();


    }
}
