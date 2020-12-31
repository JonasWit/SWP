using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClients : LegalActionsBase
    {
        public GetClients(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<Client> GetClientsWithoutData(string claim, bool active = true) => _legalManager.GetClientsWithoutCases(claim, active);
        public List<Client> GetClientsWithCleanCases(string claim, bool active = true) => _legalManager.GetClientsWithCleanCases(claim, active);
        public IEnumerable<int> GetClientCasesIds(int id) => _legalManager.GetClientCasesIds(id);
        public int CountJobsPerClient(int id) => _legalManager.CountJobsPerClient(id);
        public int CountCasesPerClient(int clientId) => _legalManager.CountCasesPerClient(clientId);
    }
}
