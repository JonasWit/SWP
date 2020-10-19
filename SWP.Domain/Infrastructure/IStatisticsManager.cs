using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface IStatisticsManager
    {
        int CountCases(int clientId);
        int CountClientContacts(int clientId);
        int CountClientJobs(int clientId);
        int CountReminders(int caseId);
        int CountNotes(int caseId);
        int CountCaseContacts(int caseId);
        int CountClients(string profile);
        List<string> GetProfiles();
    }
}
