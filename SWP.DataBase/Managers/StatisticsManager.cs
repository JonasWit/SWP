using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly ApplicationDbContext context;
        public StatisticsManager(ApplicationDbContext context) => this.context = context;

        public Task<int> CountCaseContacts(int caseId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountCases(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountClientContacts(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountClientJobs(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountClients(string profile)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountNotes(int caseId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountReminders(int caseId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetProfiles() => context.Clients.Select(x => x.ProfileClaim).Distinct().ToList();
    }
}
