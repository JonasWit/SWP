using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface IStatisticsManager
    {
        Task<int> CountCases(int clientId);
        Task<int> CountClientContacts(int clientId);
        Task<int> CountClientJobs(int clientId);
        Task<int> CountReminders(int caseId);
        Task<int> CountNotes(int caseId);
        Task<int> CountCaseContacts(int caseId);
        Task<int> CountClients(string profile);
        List<string> GetProfiles();
    }
}
