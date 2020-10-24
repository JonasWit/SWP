using SWP.Domain.Infrastructure.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.DataBase.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly ApplicationDbContext context;
        public StatisticsManager(ApplicationDbContext context) => this.context = context;

        public int CountCaseContacts(int caseId)
        {
            throw new NotImplementedException();
        }

        public int CountCases(int clientId)
        {
            throw new NotImplementedException();
        }

        public int CountClientContacts(int clientId)
        {
            throw new NotImplementedException();
        }

        public int CountClientJobs(int clientId)
        {
            throw new NotImplementedException();
        }

        public int CountClients(string profile) => context.Clients.Where(x => x.ProfileClaim == profile).Count();

        public int CountNotes(int caseId)
        {
            throw new NotImplementedException();
        }

        public int CountReminders(int caseId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetProfiles() => context.Clients.Select(x => x.ProfileClaim).Distinct().ToList();
    }
}
